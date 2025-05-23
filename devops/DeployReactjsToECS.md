AWS ECS Terraform Deployment Documentation
This document explains the Terraform configuration (main.tf) used to deploy a ReactJS frontend application on AWS ECS Fargate, integrated with an Application Load Balancer (ALB), a custom VPC, and an Amazon ECR repository. The deployment is in the us-east-1 region, and the configuration addresses challenges encountered during setup, including Elastic Beanstalk errors, ECS networking issues, missing ECR images, and a 503 error due to multiple task ARNs.
Prerequisites

AWS CLI: Configured with credentials for account <account_id> e.g 354042823355.
Terraform: Version 1.12.x installed.
Docker: For building and pushing the React app image.
Project Structure:
React app: /c/Workspace/backend-note-book/devops/front-end/
Terraform: /c/Workspace/backend-note-book/devops/terraform/ecs/


Dockerfile: In devops/front-end/ to containerize the React app.
Node.js and npm: For building the React app.

Terraform Configuration (main.tf)
Below is the complete main.tf file with a line-by-line explanation of each section.
```

provider "aws" {
  region = var.aws_region
  max_retries = 3
}

# VPC
resource "aws_vpc" "main" {
  cidr_block           = "10.0.0.0/16"
  enable_dns_hostnames = true
  enable_dns_support   = true
  tags                 = { Name = "react-vpc" }
}

resource "aws_subnet" "public" {
  count                   = 2
  vpc_id                  = aws_vpc.main.id
  cidr_block              = "10.0.${count.index}.0/24"
  availability_zone       = element(data.aws_availability_zones.available.names, count.index)
  map_public_ip_on_launch = true
  tags                    = { Name = "react-public-subnet-${count.index}" }
}

data "aws_availability_zones" "available" {
  state = "available"
}

# Internet Gateway
resource "aws_internet_gateway" "main" {
  vpc_id = aws_vpc.main.id
  tags   = { Name = "react-igw" }
}

# Route Table
resource "aws_route_table" "public" {
  vpc_id = aws_vpc.main.id
  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.main.id
  }
  tags = { Name = "react-public-rt" }
}

# Route Table Association
resource "aws_route_table_association" "public" {
  count          = 2
  subnet_id      = aws_subnet.public[count.index].id
  route_table_id = aws_route_table.public.id
}

# ECS Cluster
resource "aws_ecs_cluster" "react_cluster" {
  name = "react-cluster"
}

# Task Definition
resource "aws_ecs_task_definition" "react_task" {
  family                   = "react-frontend"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_task_execution.arn
  container_definitions = jsonencode([{
    name      = "react-container"
    image     = "${aws_ecr_repository.react_repo.repository_url}:latest"
    essential = true
    portMappings = [{
      containerPort = 80
      hostPort      = 80
      protocol      = "tcp"
    }]
    logConfiguration = {
      logDriver = "awslogs"
      options = {
        "awslogs-group"         = "/ecs/react-frontend"
        "awslogs-region"        = var.aws_region
        "awslogs-stream-prefix" = "ecs"
      }
    }
  }])
}

# CloudWatch Log Group
resource "aws_cloudwatch_log_group" "react_log_group" {
  name              = "/ecs/react-frontend"
  retention_in_days = 7
}

# ECR Repository
resource "aws_ecr_repository" "react_repo" {
  name = "react-frontend"
}

# IAM Role for ECS Task Execution
resource "aws_iam_role" "ecs_task_execution" {
  name = "ecsTaskExecutionRole"
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Action    = "sts:AssumeRole"
      Principal = { Service = "ecs-tasks.amazonaws.com" }
      Effect    = "Allow"
    }]
  })
}

resource "aws_iam_role_policy_attachment" "ecs_task_execution_policy" {
  role       = aws_iam_role.ecs_task_execution.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

resource "aws_iam_role_policy" "ecs_logs_policy" {
  name   = "ecs_logs_policy"
  role   = aws_iam_role.ecs_task_execution.id
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "logs:CreateLogStream",
          "logs:PutLogEvents"
        ]
        Resource = "${aws_cloudwatch_log_group.react_log_group.arn}:*"
      }
    ]
  })
}

# ALB
resource "aws_lb" "react_alb" {
  name               = "react-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.alb_sg.id]
  subnets            = aws_subnet.public[*].id
  tags               = { Name = "react-alb" }
  timeouts {
    create = "2m"
  }
}

resource "aws_security_group" "alb_sg" {
  vpc_id = aws_vpc.main.id
  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  tags = { Name = "react-alb-sg" }
}

resource "aws_lb_target_group" "react_tg" {
  name        = "react-tg"
  port        = 80
  protocol    = "HTTP"
  vpc_id      = aws_vpc.main.id
  target_type = "ip"
  health_check {
    path                = "/index.html"
    protocol            = "HTTP"
    matcher             = "200"
    interval            = 30
    timeout             = 5
    healthy_threshold   = 2
    unhealthy_threshold = 2
  }
}

resource "aws_lb_listener" "react_listener" {
  load_balancer_arn = aws_lb.react_alb.arn
  port              = 80
  protocol          = "HTTP"
  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.react_tg.arn
  }
}

# ECS Service
resource "aws_ecs_service" "react_service" {
  name            = "react-service"
  cluster         = aws_ecs_cluster.react_cluster.id
  task_definition = aws_ecs_task_definition.react_task.arn
  desired_count   = 1
  launch_type     = "FARGATE"
  network_configuration {
    subnets          = aws_subnet.public[*].id
    security_groups  = [aws_security_group.ecs_sg.id]
    assign_public_ip = true
  }
  load_balancer {
    target_group_arn = aws_lb_target_group.react_tg.arn
    container_name   = "react-container"
    container_port   = 80
  }
  depends_on = [aws_lb_listener.react_listener]
  timeouts {
    create = "2m"
  }
}

resource "aws_security_group" "ecs_sg" {
  vpc_id = aws_vpc.main.id
  ingress {
    from_port       = 80
    to_port         = 80
    protocol        = "tcp"
    security_groups = [aws_security_group.alb_sg.id]
  }
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  tags = { Name = "react-ecs-sg" }
}

# Outputs
output "alb_url" {
  value       = "http://${aws_lb.react_alb.dns_name}"
  description = "Use this URL for testing"
}

output "ecr_id" {
  value       = aws_ecr_repository.react_repo.id
  description = "ECR repo id"
}

output "vpc_id" {
  value       = aws_vpc.main.id
  description = "VPC ID for debugging"
}

output "subnet_ids" {
  value       = aws_subnet.public[*].id
  description = "Subnet IDs for debugging"
}

output "internet_gateway_id" {
  value       = aws_internet_gateway.main.id
  description = "Internet Gateway ID for debugging"
}
```

Line-by-Line Explanation
Provider Configuration
provider "aws" {
  region = var.aws_region
  max_retries = 3
}


Line 1: Declares the AWS provider for Terraform to interact with AWS services.
Line 2: Sets the AWS region from a variable aws_region (default: us-east-1).
Line 3: Limits API retries to 3 for transient errors, addressing your request to fail fast and save time (resolving prolonged ALB creation issues).

VPC Configuration
```
resource "aws_vpc" "main" {
  cidr_block           = "10.0.0.0/16"
  enable_dns_hostnames = true
  enable_dns_support   = true
  tags                 = { Name = "react-vpc" }
}
```

Line 1: Defines a VPC resource named main.
Line 2: Sets the CIDR block to 10.0.0.0/16 (65,536 IP addresses).
Line 3–4: Enables DNS hostnames and support for ECS task networking.
Line 5: Tags the VPC for identification.

```
resource "aws_subnet" "public" {
  count                   = 2
  vpc_id                  = aws_vpc.main.id
  cidr_block              = "10.0.${count.index}.0/24"
  availability_zone       = element(data.aws_availability_zones.available.names, count.index)
  map_public_ip_on_launch = true
  tags                    = { Name = "react-public-subnet-${count.index}" }
}
```

Line 1: Defines public subnets (2 instances via count).
Line 2: Creates two subnets (count.index = 0, 1).
Line 3: Associates subnets with the VPC.
Line 4: Sets CIDR blocks (10.0.0.0/24, 10.0.1.0/24).
Line 5: Places subnets in different Availability Zones for high availability.
Line 6: Assigns public IPs to instances (required for Fargate).
Line 7: Tags subnets for identification.

```
data "aws_availability_zones" "available" {
  state = "available"
}
```


Line 1: Fetches available AZs in us-east-1.
Line 2: Filters for available AZs, ensuring valid subnet placement.

Internet Gateway and Routing
```
resource "aws_internet_gateway" "main" {
  vpc_id = aws_vpc.main.id
  tags   = { Name = "react-igw" }
}
```

Line 1: Creates an Internet Gateway for public internet access.
Line 2: Attaches it to the VPC (fixes the InvalidSubnet: VPC has no internet gateway error).
Line 3: Tags the gateway.

```
resource "aws_route_table" "public" {
  vpc_id = aws_vpc.main.id
  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.main.id
  }
  tags = { Name = "react-public-rt" }
}
```


Line 1: Defines a public route table.
Line 2: Associates it with the VPC.
Line 3–5: Adds a route sending all traffic (0.0.0.0/0) to the Internet Gateway.
Line 6: Tags the route table.

```
resource "aws_route_table_association" "public" {
  count          = 2
  subnet_id      = aws_subnet.public[count.index].id
  route_table_id = aws_route_table.public.id
}
```

Line 1: Associates subnets with the public route table.
Line 2: Applies to both subnets.
Line 3: Links each subnet to the route table.
Line 4: Specifies the route table ID.

ECS Cluster
```
resource "aws_ecs_cluster" "react_cluster" {
  name = "react-cluster"
}
```

Line 1: Creates an ECS cluster.
Line 2: Names it react-cluster to host the service.

Task Definition
```
resource "aws_ecs_task_definition" "react_task" {
  family                   = "react-frontend"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_task_execution.arn
  container_definitions = jsonencode([{
    name      = "react-container"
    image     = "${aws_ecr_repository.react_repo.repository_url}:latest"
    essential = true
    portMappings = [{
      containerPort = 80
      hostPort      = 80
      protocol      = "tcp"
    }]
    logConfiguration = {
      logDriver = "awslogs"
      options = {
        "awslogs-group"         = "/ecs/react-frontend"
        "awslogs-region"        = var.aws_region
        "awslogs-stream-prefix" = "ecs"
      }
    }
  }])
}
```

Line 1: Defines an ECS task definition.
Line 2: Names the task family react-frontend.
Line 3: Uses awsvpc mode for Fargate networking.
Line 4: Specifies Fargate compatibility.
Line 5–6: Allocates 256 vCPU units and 512 MB memory.
Line 7: Assigns the task execution IAM role.
Line 8–22: Defines a container:
Line 9: Names it react-container.
Line 10: Uses the ECR image react-frontend:latest (resolves missing image issue).
Line 11: Marks the container as essential (task stops if it fails).
Line 12–15: Maps port 80 for HTTP traffic.
Line 16–21: Configures CloudWatch Logs for container output.

CloudWatch Log Group
```
resource "aws_cloudwatch_log_group" "react_log_group" {
  name              = "/ecs/react-frontend"
  retention_in_days = 7
}
```

Line 1: Creates a CloudWatch Log Group.
Line 2: Names it /ecs/react-frontend.
Line 3: Sets log retention to 7 days.

ECR Repository
```
resource "aws_ecr_repository" "react_repo" {
  name = "react-frontend"
}
```

Line 1: Creates an ECR repository.
Line 2: Names it react-frontend (URI: 354042823355.dkr.ecr.us-east-1.amazonaws.com/react-frontend).

IAM Role for ECS Task Execution
```
resource "aws_iam_role" "ecs_task_execution" {
  name = "ecsTaskExecutionRole"
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Action    = "sts:AssumeRole"
      Principal = { Service = "ecs-tasksamazon.com" }
      Effect    = "Allow"
    }]
  })
}
```

Line 1: Creates an IAM role for ECS tasks.
Line 2: Names it ecsTaskExecutionRole.
Line 3–9: Allows ecs-tasks.amazonaws.com to assume the role (fixed typo from amazon.com).

```
resource "aws_iam_role_policy_attachment" "ecs_task_execution_policy" {
  role       = aws_iam_role.ecs_task_execution.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}
```


Line 1: Attaches a managed policy to the role.
Line 2: Specifies the role.
Line 3: Grants permissions for ECR and CloudWatch (e.g., pulling images).

```
resource "aws_iam_role_policy" "ecs_logs_policy" {
  name   = "ecs_logs_policy"
  role   = aws_iam_role.ecs_task_execution.id
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "logs:CreateLogStream",
          "logs:PutLogEvents"
        ]
        Resource = "${aws_cloudwatch_log_group.react_log_group.arn}:*"
      }
    ]
  })
}
```

Line 1: Defines a custom policy for CloudWatch Logs.
Line 2: Names it ecs_logs_policy.
Line 3: Attaches to the role.
Line 4–12: Grants permissions to create and write log streams.

ALB Configuration
```
resource "aws_lb" "react_alb" {
  name               = "react-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.alb_sg.id]
  subnets            = aws_subnet.public[*].id
  tags               = { Name = "react-alb" }
  timeouts {
    create = "2m"
  }
}
```


Line 1: Creates an ALB.
Line 2: Names it react-alb.
Line 3: Makes it public-facing.
Line 4: Specifies Application Load Balancer type.
Line 5: Associates with the ALB security group.
Line 6: Places in public subnets.
Line 7: Tags the ALB.
Line 8–9: Sets a 2-minute creation timeout (addresses retry issues).

```
resource "aws_security_group" "alb_sg" {
  vpc_id = aws_vpc.main.id
  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  tags = { Name = "react-alb-sg" }
}
```


Line 1: Creates a security group for the ALB.
Line 2: Associates with the VPC.
Line 3–6: Allows inbound HTTP traffic on port 80 from anywhere.
Line 7–10: Allows all outbound traffic.
Line 11: Tags the security group.

```
resource "aws_lb_target_group" "react_tg" {
  name        = "react-tg"
  port        = 80
  protocol    = "HTTP"
  vpc_id      = aws_vpc.main.id
  target_type = "ip"
  health_check {
    path                = "/index.html"
    protocol            = "HTTP"
    matcher             = "200"
    interval            = 30
    timeout             = 5
    healthy_threshold   = 2
    unhealthy_threshold = 2
  }
}
```


Line 1: Creates a target group for the ALB.
Line 2: Names it react-tg.
Line 3–4: Configures for HTTP on port 80.
Line 5: Associates with the VPC.
Line 6: Uses ip targets for Fargate.
Line 7–13: Defines health checks for /index.html (fixes 503 error by matching React’s serving path).
Checks every 30 seconds, times out after 5 seconds, requires 2 successes or failures.



```
resource "aws_lb_listener" "react_listener" {
  load_balancer_arn = aws_lb.react_alb.arn
  port              = 80
  protocol          = "HTTP"
  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.react_tg.arn
  }
}
```


Line 1: Creates an ALB listener.
Line 2: Associates with the ALB.
Line 3–4: Listens on port 80 for HTTP.
Line 5–7: Forwards traffic to the target group.

ECS Service
```
resource "aws_ecs_service" "react_service" {
  name            = "react-service"
  cluster         = aws_ecs_cluster.react_cluster.id
  task_definition = aws_ecs_task_definition.react_task.arn
  desired_count   = 1
  launch_type     = "FARGATE"
  network_configuration {
    subnets          = aws_subnet.public[*].id
    security_groups  = [aws_security_group.ecs_sg.id]
    assign_public_ip = true
  }
  load_balancer {
    target_group_arn = aws_lb_target_group.react_tg.arn
    container_name   = "react-container"
    container_port   = 80
  }
  depends_on = [aws_lb_listener.react_listener]
  timeouts {
    create = "2m"
  }
}
```
Line 1: Creates an ECS service.
Line 2: Names it react-service.
Line 3: Associates with the ECS cluster.
Line 4: Uses the task definition.
Line 5: Sets desired_count = 1 (fixes two task ARNs issue).
Line 6: Uses Fargate launch type.
Line 7–10: Configures networking:
Runs in public subnets.
Uses the ECS security group.
Assigns public IPs for internet access.

Line 11–14: Integrates with the ALB target group, forwarding port 80 traffic.
Line 15: Ensures the ALB listener is created first.
Line 16–17: Sets a 2-minute creation timeout.

```
resource "aws_security_group" "ecs_sg" {
  vpc_id = aws_vpc.main.id
  ingress {
    from_port       = 80
    to_port         = 80
    protocol        = "tcp"
    security_groups = [aws_security_group.alb_sg.id]
  }
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  tags = { Name = "react-ecs-sg" }
}
```


Line 1: Creates a security group for ECS tasks.
Line 2: Associates with the VPC.
Line 3–6: Allows inbound port 80 traffic from the ALB security group.
Line 7–10: Allows all outbound traffic.
Line 11: Tags the security group.

Outputs
```
output "alb_url" {
  value       = "http://${aws_lb.react_alb.dns_name}"
  description = "Use this URL for testing"
}
```

Line 1–3: Outputs the ALB URL (e.g., http://react-alb-1961836355.us-east-1.elb.amazonaws.com).

```
output "ecr_id" {
  value       = aws_ecr_repository.react_repo.id
  description = "ECR repo id"
}
```


Line 1–3: Outputs the ECR repository name (react-frontend).

```
output "vpc_id" {
  value       = aws_vpc.main.id
  description = "VPC ID for debugging"
}
```

Line 1–3: Outputs the VPC ID for debugging.

```
output "subnet_ids" {
  value       = aws_subnet.public[*].id
  description = "Subnet IDs for debugging"
}
```


Line 1–3: Outputs the public subnet IDs.

```
output "internet_gateway_id" {
  value       = aws_internet_gateway.main.id
  description = "Internet Gateway ID for debugging"
}
```


Line 1–3: Outputs the Internet Gateway ID.

Deployment Steps

Prepare the React App:

In /c/Workspace/backend-note-book/devops/front-end/:npm install
```
npm run build
```

Ensure Dockerfile:
```
FROM node:18-alpine
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm ci
COPY . .
RUN npm run build
RUN npm install -g serve
EXPOSE 80
CMD ["serve", "-s", "build", "-l", "80"]
```

Build and Push Docker Image:
```
cd /c/Workspace/backend-note-book/devops/front-end
docker build -t react-frontend .
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 354042823355.dkr.ecr.us-east-1.amazonaws.com
docker tag react-frontend:latest 354042823355.dkr.ecr.us-east-1.amazonaws.com/react-frontend:latest
docker push 354042823355.dkr.ecr.us-east-1.amazonaws.com/react-frontend:latest
```

Apply Terraform:
```
cd /c/Workspace/backend-note-book/devops/terraform/ecs
terraform init
terraform apply
```

Test the App: 

Access the ALB URL (e.g., http://react-alb-1961836355.us-east-1.elb.amazonaws.com).
Verify logs on CMD or Powershell:
```
aws logs tail /ecs/react-frontend --region us-east-1
```

Cleanup:
```
terraform destroy
aws ecr delete-repository --repository-name react-frontend --region us-east-1 --force
```
Challenges and Resolutions

Elastic Beanstalk Errors: Initial attempts failed due to IAM permissions, S3 issues, and environment creation retries (couldn't find resource). Switched to ECS for simplicity.
ECS Networking: Fixed InvalidSubnet: VPC has no internet gateway by adding an Internet Gateway and route table.
Missing ECR Image: Resolved 503 error by pushing react-frontend:latest to ECR.
Two Task ARNs: Fixed by setting desired_count = 1 and forcing a new deployment.
503 Error: Addressed by updating the health check to /index.html and ensuring healthy tasks.

Conclusion
This Terraform configuration provides a robust setup for deploying a ReactJS frontend on AWS ECS Fargate, with lessons learned from debugging networking, IAM, and ECS task issues. The line-by-line explanation ensures clarity for future maintenance or scaling.
