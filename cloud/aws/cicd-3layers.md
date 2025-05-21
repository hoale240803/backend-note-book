Deploying a Web Application on AWS with Terraform
This document provides a step-by-step guide to deploy a web application using AWS, Terraform, ReactJS, .NET, PostgreSQL, SonarQube, and GitHub Actions for CI/CD. The infrastructure is provisioned on AWS EC2 and RDS, with code quality checks and automated deployment pipelines.
Prerequisites

AWS Account: Ensure you have an AWS account with appropriate permissions.
Terraform: Install Terraform CLI (v1.5.0 or later) on your local machine.
AWS CLI: Install and configure the AWS CLI with your credentials.
GitHub Repository: A repository containing the ReactJS frontend and .NET backend code.
Node.js and .NET SDK: Installed for local development and testing.
Docker: For running SonarQube locally or in CI/CD.
Basic Knowledge: Familiarity with AWS, Terraform, ReactJS, .NET, and CI/CD concepts.

Step-by-Step Guide
Step 1: Project Structure
Organize your project directory as follows:
project/
├── frontend/                # ReactJS frontend code
├── backend/                 # .NET backend code
├── terraform/               # Terraform configuration files
├── sonarqube/              # SonarQube configuration
└── .github/workflows/       # GitHub Actions CI/CD workflows

Step 2: Set Up the ReactJS Frontend

Create the React App:

Run npx create-react-app frontend to initialize the React project.
Build the app for production: cd frontend && npm run build.
The build/ directory will be served by an EC2 instance with Nginx.


Test Locally:

Run npm start to verify the frontend works locally (default: http://localhost:3000).



Step 3: Set Up the .NET Backend

Create the .NET API:

Run dotnet new webapi -o backend to create a .NET Core Web API.
Add necessary packages (e.g., Entity Framework Core for database access):dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL


Implement API endpoints and configure connection to PostgreSQL.


Test Locally:

Run dotnet run to start the backend (default: http://localhost:5000).
Test API endpoints using tools like Postman or cURL.



Step 4: Set Up SonarQube for Code Quality

Run SonarQube Locally with Docker:

Pull and run SonarQube:docker run -d --name sonarqube -p 9000:9000 sonarqube:latest


Access SonarQube at http://localhost:9000 (default login: admin/admin).


Configure SonarQube Scanner:

Install the SonarQube Scanner CLI.
Create a sonar-project.properties file in both frontend/ and backend/:sonar.projectKey=my-project
sonar.projectName=My Project
sonar.host.url=http://localhost:9000
sonar.login=admin
sonar.password=admin
sonar.sourceEncoding=UTF-8


Run analysis:sonar-scanner -Dproject.settings=sonar-project.properties





Step 5: Write Terraform Configuration

Create Terraform Directory:

Create a terraform/ directory with the following files:
main.tf: Core infrastructure configuration.
variables.tf: Variable definitions.
outputs.tf: Output values for reference.




Define AWS Infrastructure:

VPC and Networking: Set up a VPC, subnets, and security groups.
EC2 Instances: Provision instances for frontend and backend.
RDS Instance: Set up a PostgreSQL database.
IAM Roles: For EC2 to access other AWS services.

Example main.tf:
provider "aws" {
  region = var.aws_region
}

# VPC
resource "aws_vpc" "main" {
  cidr_block = "10.0.0.0/16"
  enable_dns_support = true
  enable_dns_hostnames = true
  tags = { Name = "app-vpc" }
}

# Subnets
resource "aws_subnet" "public" {
  vpc_id = aws_vpc.main.id
  cidr_block = "10.0.1.0/24"
  availability_zone = "${var.aws_region}a"
  map_public_ip_on_launch = true
  tags = { Name = "public-subnet" }
}

# Internet Gateway
resource "aws_internet_gateway" "main" {
  vpc_id = aws_vpc.main.id
  tags = { Name = "app-igw" }
}

# Route Table
resource "aws_route_table" "public" {
  vpc_id = aws_vpc.main.id
  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.main.id
  }
  tags = { Name = "public-route-table" }
}

resource "aws_route_table_association" "public" {
  subnet_id = aws_subnet.public.id
  route_table_id = aws_route_table.public.id
}

# Security Groups
resource "aws_security_group" "web_sg" {
  vpc_id = aws_vpc.main.id
  ingress {
    from_port = 80
    to_port = 80
    protocol = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  ingress {
    from_port = 22
    to_port = 22
    protocol = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  egress {
    from_port = 0
    to_port = 0
    protocol = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  tags = { Name = "web-sg" }
}

resource "aws_security_group" "rds_sg" {
  vpc_id = aws_vpc.main.id
  ingress {
    from_port = 5432
    to_port = 5432
    protocol = "tcp"
    security_groups = [aws_security_group.web_sg.id]
  }
  egress {
    from_port = 0
    to_port = 0
    protocol = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  tags = { Name = "rds-sg" }
}

# EC2 for Frontend (Nginx serving React)
resource "aws_instance" "frontend" {
  ami = var.ami_id
  instance_type = "t2.micro"
  subnet_id = aws_subnet.public.id
  security_groups = [aws_security_group.web_sg.id]
  user_data = <<-EOF
    #!/bin/bash
    yum update -y
    yum install -y nginx
    systemctl start nginx
    systemctl enable nginx
    rm -rf /usr/share/nginx/html/*
    # Copy React build files (assumes uploaded via CI/CD)
    # Placeholder for copying build files
    systemctl restart nginx
  EOF
  tags = { Name = "frontend-ec2" }
}

# EC2 for Backend (.NET)
resource "aws_instance" "backend" {
  ami = var.ami_id
  instance_type = "t2.micro"
  subnet_id = aws_subnet.public.id
  security_groups = [aws_security_group.web_sg.id]
  user_data = <<-EOF
    #!/bin/bash
    yum update -y
    curl -o- https://dot.net/v1/dotnet-install.sh | bash
    export DOTNET_ROOT=$HOME/.dotnet
    export PATH=$PATH:$HOME/.dotnet
    # Deploy .NET app (assumes uploaded via CI/CD)
    # Placeholder for deploying .NET app
  EOF
  tags = { Name = "backend-ec2" }
}

# RDS PostgreSQL
resource "aws_db_instance" "postgres" {
  identifier = "app-db"
  engine = "postgres"
  engine_version = "13.7"
  instance_class = "db.t3.micro"
  allocated_storage = 20
  username = var.db_username
  password = var.db_password
  vpc_security_group_ids = [aws_security_group.rds_sg.id]
  db_subnet_group_name = aws_db_subnet_group.main.name
  skip_final_snapshot = true
  tags = { Name = "app-db" }
}

resource "aws_db_subnet_group" "main" {
  name = "main"
  subnet_ids = [aws_subnet.public.id]
  tags = { Name = "db-subnet-group" }
}


Define Variables (variables.tf):
variable "aws_region" {
  description = "AWS region"
  default = "us-east-1"
}

variable "ami_id" {
  description = "AMI ID for EC2 instances"
  default = "ami-0c55b159cbfafe1f0" # Amazon Linux 2 AMI (update as needed)
}

variable "db_username" {
  description = "RDS database username"
  sensitive = true
}

variable "db_password" {
  description = "RDS database password"
  sensitive = true
}


Define Outputs (outputs.tf):
output "frontend_url" {
  value = aws_instance.frontend.public_ip
}

output "backend_url" {
  value = aws_instance.backend.public_ip
}

output "rds_endpoint" {
  value = aws_db_instance.postgres.endpoint
}


Initialize and Apply Terraform:

Run terraform init in the terraform/ directory.
Create a terraform.tfvars file with sensitive variables:db_username = "admin"
db_password = "your-secure-password"


Run terraform plan to review the infrastructure plan.
Run terraform apply to provision the resources.



Step 6: Set Up CI/CD with GitHub Actions

Create a Workflow File:

In .github/workflows/deploy.yml, define the CI/CD pipeline:name: CI/CD Pipeline

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3

    - name: Set up Node.js
      uses: actions/setup-node@v3
      with:
        node-version: '16'

    - name: Build Frontend
      run: |
        cd frontend
        npm install
        npm run build

    - name: Set up .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '6.0.x'

    - name: Build Backend
      run: |
        cd backend
        dotnet build

    - name: Run SonarQube Analysis
      env:
        SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}
      run: |
        dotnet tool install --global dotnet-sonarscanner
        dotnet sonarscanner begin /k:"my-project" /d:sonar.host.url="http://<sonarqube-url>" /d:sonar.login="${{ secrets.SONAR_TOKEN }}"
        dotnet build backend
        npm run test --prefix frontend
        dotnet sonarscanner end /d:sonar.login="${{ secrets.SONAR_TOKEN }}"

    - name: Deploy to EC2
      env:
        AWS_ACCESS_KEY_ID: ${{ secrets.AWS_ACCESS_KEY_ID }}
        AWS_SECRET_ACCESS_KEY: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
      run: |
        # Deploy frontend to EC2
        scp -i ${{ secrets.EC2_SSH_KEY }} -r frontend/build/* ec2-user@${{ secrets.FRONTEND_EC2_IP }}:/usr/share/nginx/html/
        # Deploy backend to EC2
        scp -i ${{ secrets.EC2_SSH_KEY }} -r backend/bin/Release/net6.0/* ec2-user@${{ secrets.BACKEND_EC2_IP }}:/home/ec2-user/app/
        ssh -i ${{ secrets.EC2_SSH_KEY }} ec2-user@${{ secrets.BACKEND_EC2_IP }} "cd /home/ec2-user/app && dotnet run"




Configure Secrets in GitHub:

Add the following secrets in your GitHub repository settings:
AWS_ACCESS_KEY_ID: AWS access key.
AWS_SECRET_ACCESS_KEY: AWS secret key.
EC2_SSH_KEY: SSH key for EC2 instances.
FRONTEND_EC2_IP: Public IP of the frontend EC2 instance.
BACKEND_EC2_IP: Public IP of the backend EC2 instance.
SONAR_TOKEN: SonarQube authentication token.





Step 7: Test and Verify

Access the Application:

Frontend: Access http://<frontend-ec2-public-ip> to verify the React app.
Backend: Test API endpoints at http://<backend-ec2-public-ip>:5000.
Database: Connect to the RDS endpoint using a PostgreSQL client.


Verify SonarQube:

Check the SonarQube dashboard for code quality metrics and issues.


Verify CI/CD:

Push changes to the main branch and monitor the GitHub Actions workflow.
Ensure the application is deployed and accessible.



Step 8: Handover and Documentation

Share the Repository: Provide access to the GitHub repository containing all code and Terraform configurations.
Document Access:
AWS credentials (stored securely).
SSH keys for EC2 instances.
SonarQube login details.
GitHub repository secrets.


Train Team Members:
Walk through the Terraform setup and CI/CD pipeline.
Demonstrate how to run SonarQube scans and interpret results.



Troubleshooting

Terraform Errors: Check the AWS CLI configuration and ensure the AMI ID is valid for your region.
CI/CD Failures: Verify GitHub secrets and SSH connectivity to EC2 instances.
SonarQube Issues: Ensure the SonarQube server is running and accessible.

Next Steps

Add monitoring with AWS CloudWatch.
Implement auto-scaling for EC2 instances.
Enhance security with AWS WAF and SSL/TLS certificates.

