provider "aws" {
  region = var.aws_region
  # Limit retries for API errors
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