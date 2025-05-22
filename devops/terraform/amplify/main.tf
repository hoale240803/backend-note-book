provider "aws" {
  region = var.aws_region
}

# Amplify App
resource "aws_amplify_app" "react_app" {
  name                     = "react-frontend"
  repository               = var.repository
  access_token             = var.github_token
  build_spec               = <<-EOT
    version: 1
    frontend:
      phases:
        preBuild:
          commands:
            - cd devops/front-end
            - npm ci # Use npm ci for consistent installs
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

# Amplify Branch
resource "aws_amplify_branch" "main" {
  app_id      = aws_amplify_app.react_app.id
  branch_name = "main"
}

# Output
output "amplify_url" {
  value       = "https://${aws_amplify_branch.main.branch_name}.amplifyapp.com"
  description = "Use this URL (e.g., https://main.<app-id>.amplifyapp.com) for testing"
}
