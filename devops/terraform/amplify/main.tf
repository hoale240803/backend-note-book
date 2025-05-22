provider "aws" {
  region = var.aws_region
}

# IAM Role for Amplify
resource "aws_iam_role" "amplify_service_role" {
  name = "amplify-service-role-${random_string.suffix.result}"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect    = "Allow"
        Principal = { Service = "amplify.amazonaws.com" }
        Action    = "sts:AssumeRole"
      }
    ]
  })
}

# Random suffix to avoid role name conflicts
resource "random_string" "suffix" {
  length  = 8
  special = false
  upper   = false
}

# Custom IAM Policy for Amplify
resource "aws_iam_policy" "amplify_policy" {
  name        = "amplify-build-policy"
  description = "Policy for Amplify build and deployment"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "s3:GetObject",
          "s3:PutObject",
          "s3:DeleteObject",
          "s3:ListBucket"
        ]
        Resource = [
          "arn:aws:s3:::amplify-*",
          "arn:aws:s3:::amplify-*/*"
        ]
      },
      {
        Effect   = "Allow"
        Action   = [
          "cloudfront:CreateInvalidation",
          "cloudfront:GetInvalidation",
          "cloudfront:ListDistributions"
        ]
        Resource = "*"
      },
      {
        Effect   = "Allow"
        Action   = [
          "amplify:StartJob",
          "amplify:GetJob",
          "amplify:ListJobs",
          "amplify:UpdateApp",
          "amplify:GetApp"
        ]
        Resource = "arn:aws:amplify:${var.aws_region}:354042823355:apps/*"
      }
    ]
  })
}

# Attach Policy to Role
resource "aws_iam_role_policy_attachment" "amplify_policy_attachment" {
  role       = aws_iam_role.amplify_service_role.name
  policy_arn = aws_iam_policy.amplify_policy.arn
}

# Amplify App
resource "aws_amplify_app" "react_app" {
  name       = "react-frontend"
  repository = "https://github.com/lehoa/backend-note-book"
  access_token = var.github_token
  iam_service_role_arn = aws_iam_role.amplify_service_role.arn # Link IAM role

  build_spec = <<-EOT
    version: 1
    frontend:
      phases:
        preBuild:
          commands:
            - pwd
            - ls -la
            - cd devops/front-end
            - ls -la
            - npm ci
        build:
          commands:
            - npm run build
            - ls -la build
      artifacts:
        baseDirectory: devops/front-end/build
        files:
          - '**/*'
      cache:
        paths:
          - devops/front-end/node_modules/**/*
  EOT
  enable_branch_auto_build = true
}

resource "aws_amplify_branch" "main" {
  app_id      = aws_amplify_app.react_app.id
  branch_name = "main"
}