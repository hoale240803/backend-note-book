Deploying a ReactJS Frontend on AWS with Amplify
This document provides a step-by-step guide to deploy a ReactJS frontend application on AWS Amplify using Terraform for infrastructure and GitHub Actions for CI/CD. The setup uses the default Amplify domain (e.g., https://main.<app-id>.amplifyapp.com) for testing, eliminating the need for a custom domain. A .gitignore file excludes unnecessary files (e.g., node_modules, Terraform provider binaries).
Prerequisites

AWS Account: With appropriate permissions.
Terraform: CLI v1.12.0 or later.
AWS CLI: Configured with credentials (aws configure).
GitHub Repository: Containing the ReactJS code (e.g., https://github.com/lehoa/backend-note-book).
Node.js: For local development.
GitHub Token: Personal access token with repo scope.
Knowledge: AWS, Terraform, ReactJS, and CI/CD basics.

Step-by-Step Guide
Step 1: Project Structure
project/
├── devops/front-end/        # ReactJS frontend code
├── terraform/amplify/       # Amplify Terraform files
├── .github/workflows/       # GitHub Actions workflows
└── .gitignore               # Git ignore file

Step 2: Set Up .gitignore
# .gitignore
# Terraform
.terraform/
*.tfstate
*.tfstate.*
terraform.tfvars
*.tfplan
*.lock.hcl

# Node.js
devops/front-end/node_modules/
devops/front-end/.env
devops/front-end/build/
devops/front-end/dist/
*.log
npm-debug.log*

# Miscellaneous
.DS_Store
.vscode/
.idea/
*.swp
*.bak
.env
*.tmp

Step 3: Set Up the ReactJS Frontend

Create the React App:
Run npx create-react-app devops/front-end.
Build: cd devops/front-end && npm run build.


Test Locally:
Run npm start (default: http://localhost:3000).



Step 4: Write Terraform Configuration

Create Terraform Directory:

Create terraform/amplify/ with:
main.tf: Amplify app and branch configuration.
variables.tf: Variable definitions.
outputs.tf: Output values.




Define AWS Infrastructure (terraform/amplify/main.tf):
terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "5.98.0"
    }
  }
}

provider "aws" {
  region = var.aws_region
}

resource "aws_amplify_app" "react_app" {
  name       = "react-frontend"
  repository = "https://github.com/lehoa/backend-note-book"
  access_token = var.github_token
  build_spec = <<-EOT
    version: 1
    frontend:
      phases:
        preBuild:
          commands:
            - cd devops/front-end
            - npm install
        build:
          commands:
            - npm run build
      artifacts:
        baseDirectory: devops/front-end/build
        files:
          - '**/*'
      cache:
        paths:
          - devops/front-end/node_modules/**/*
  EOT
  enable_branch_auto_build = false
}

resource "aws_amplify_branch" "main" {
  app_id      = aws_amplify_app.react_app.id
  branch_name = "main"
}


Define Variables (terraform/amplify/variables.tf):
variable "aws_region" {
  description = "AWS region"
  default     = "us-east-1"
}

variable "github_token" {
  description = "GitHub personal access token for Amplify"
  type        = string
  sensitive   = true
}


Define Outputs (terraform/amplify/outputs.tf):
output "amplify_url" {
  value       = "https://${aws_amplify_branch.main.branch_name}.${aws_amplify_app.react_app.default_domain}"
  description = "Use this URL (e.g., https://main.<app-id>.amplifyapp.com) for testing"
}


Initialize and Apply Terraform:

Run terraform init in terraform/amplify/.
Create terraform.tfvars:aws_region   = "us-east-1"
github_token = "<your-github-token>"


Run terraform plan to review.
Run terraform apply and confirm with yes.
Note the amplify_url output.



Step 5: Set Up CI/CD with GitHub Actions

Create Workflow (deploy-amplify.yml):name: Notify Amplify
on:
  push:
    branches: [ main ]
jobs:
  notify:
    runs-on: ubuntu-latest
    concurrency:
      group: amplify-deploy-${{ github.ref }}
      cancel-in-progress: true
    steps:
    - uses: actions/checkout@v3
    - name: Check Amplify Build Status
      env:
        AWS_ACCESS_KEY_ID: ${{ secrets.AWS_ACCESS_KEY_ID }}
        AWS_SECRET_ACCESS_KEY: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
        AWS_DEFAULT_REGION: us-east-1
      run: |
        STATUS=$(aws amplify list-jobs --app-id


Find jobid
aws amplify list-jobs --app-id d3p1se556sejea --branch-name main

Stop running job id, we disable on amplify. Just allow Github action deploy
aws amplify stop-job --app-id d3p1se556sejea --branch-name main --job-id 1