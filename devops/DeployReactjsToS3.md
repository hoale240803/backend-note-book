Deploying a ReactJS Frontend on AWS with S3 and CloudFront
This document provides a step-by-step guide to deploy a ReactJS frontend application on AWS using Amazon S3 for static website hosting and CloudFront for content delivery. Terraform provisions the infrastructure, and GitHub Actions automates deployment. The setup uses the default CloudFront domain name (e.g., d1234567890abcdef.cloudfront.net) for testing, eliminating the need to purchase a custom domain. A .gitignore file ensures unnecessary files (e.g., node_modules, Terraform provider binaries) are excluded from version control.
Prerequisites

AWS Account: Ensure you have an AWS account with appropriate permissions.
Terraform: Install Terraform CLI (v1.5.0 or later) on your local machine.
AWS CLI: Install and configure the AWS CLI with your credentials (aws configure).
GitHub Repository: A repository for the ReactJS frontend code.
Node.js: Installed for local development and testing.
Basic Knowledge: Familiarity with AWS, Terraform, ReactJS, and CI/CD concepts.

Step-by-Step Guide
Step 1: Project Structure
Organize your project directory as follows:

```
project/
├── frontend/                # ReactJS frontend code
├── terraform/               # Terraform configuration files
├── .github/workflows/       # GitHub Actions CI/CD workflows
└── .gitignore               # Git ignore file
```

Step 2: Set Up .gitignore
Create a .gitignore file in the root directory to exclude unnecessary files:
```

# .gitignore
# Terraform
.terraform/
*.tfstate
*.tfstate.*
terraform.tfvars
*.tfplan
*.lock.hcl

# Node.js
frontend/node_modules/
frontend/.env
frontend/build/
frontend/dist/
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
```

This ensures sensitive files (e.g., terraform.tfvars), build artifacts (e.g., node_modules, build), and Terraform provider binaries are not committed.
Step 3: Set Up the ReactJS Frontend

Create the React App:

Run npx create-react-app frontend to initialize the React project.
Build the app for production: cd frontend && npm run build.
The build/ directory contains static files for S3 (ignored by .gitignore).


Test Locally:

Run npm start to verify the frontend works locally (default: http://localhost:3000).


Configure for S3:

The Terraform setup configures S3 to redirect all requests to index.html for single-page app (SPA) routing, ensuring navigation works correctly.



Step 4: Write Terraform Configuration

Create Terraform Directory:

Create a terraform/ directory with:
main.tf: Core infrastructure configuration.
variables.tf: Variable definitions.
outputs.tf: Output values.




Define AWS Infrastructure (main.tf):

S3 Bucket: Hosts static files with website hosting enabled.
CloudFront Distribution: Serves the S3 bucket with HTTPS using the default CloudFront domain (e.g., d1234567890abcdef.cloudfront.net).
IAM Policy: Grants CloudFront access to S3 via an Origin Access Identity (OAI).

```
provider "aws" {
  region = var.aws_region
}
```

# S3 Bucket for React App
```

resource "aws_s3_bucket" "frontend_bucket" {
  bucket = var.bucket_name
  tags = { Name = "react-frontend-bucket" }
}

resource "aws_s3_bucket_website_configuration" "frontend_website" {
  bucket = aws_s3_bucket.frontend_bucket.id
  index_document {
    suffix = "index.html"
  }
  error_document {
    key = "index.html" # Redirect errors to index.html for SPA routing
  }
}

resource "aws_s3_bucket_public_access_block" "frontend_public_access" {
  bucket = aws_s3_bucket.frontend_bucket.id
  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}

resource "aws_s3_bucket_policy" "frontend_policy" {
  bucket = aws_s3_bucket.frontend_bucket.id
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect    = "Allow"
        Principal = {
          AWS = aws_cloudfront_origin_access_identity.oai.iam_arn
        }
        Action    = "s3:GetObject"
        Resource  = "${aws_s3_bucket.frontend_bucket.arn}/*"
      }
    ]
  })
}
```

# CloudFront Origin Access Identity
```
resource "aws_cloudfront_origin_access_identity" "oai" {
  comment = "OAI for React frontend"
}
```

# CloudFront Distribution
```
resource "aws_cloudfront_distribution" "frontend_distribution" {
  origin {
    domain_name = aws_s3_bucket.frontend_bucket.bucket_regional_domain_name
    origin_id   = "S3-${aws_s3_bucket.frontend_bucket.id}"
    s3_origin_config {
      origin_access_identity = aws_cloudfront_origin_access_identity.oai.cloudfront_access_identity_path
    }
  }

  enabled             = true
  is_ipv6_enabled     = true
  default_root_object = "index.html"

  default_cache_behavior {
    allowed_methods  = ["GET", "HEAD"]
    cached_methods   = ["GET", "HEAD"]
    target_origin_id = "S3-${aws_s3_bucket.frontend_bucket.id}"
    forwarded_values {
      query_string = false
      cookies {
        forward = "none"
      }
    }
    viewer_protocol_policy = "redirect-to-https"
    min_ttl                = 0
    default_ttl            = 3600
    max_ttl                = 86400
  }

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  viewer_certificate {
    cloudfront_default_certificate = true
  }

  tags = { Name = "react-frontend-cloudfront" }
}
```


Define Variables (variables.tf):
```
variable "aws_region" {
  description = "AWS region"
  default     = "us-east-1"
}

variable "bucket_name" {
  description = "Name of the S3 bucket (must be globally unique)"
  type        = string
}

```

Define Outputs (outputs.tf):
```
output "s3_bucket_name" {
  value = aws_s3_bucket.frontend_bucket.bucket
}

output "cloudfront_domain_name" {
  value = aws_cloudfront_distribution.frontend_distribution.domain_name
  description = "Use this domain (e.g., https://<domain>.cloudfront.net) to access the app for testing"
}
```

Initialize and Apply Terraform:

Run terraform init in the terraform/ directory.
Create a terraform.tfvars file with a unique bucket name:aws_region  = "us-east-1"
bucket_name = "myapp-frontend-20250521" # Replace with a globally unique name


Run terraform plan to review the infrastructure.
Run terraform apply to provision the S3 bucket and CloudFront distribution.
Note the cloudfront_domain_name output for testing (e.g., d1234567890abcdef.cloudfront.net).



Step 5: Set Up CI/CD with GitHub Actions

Create a Workflow File:

In .github/workflows/deploy.yml, define the CI/CD pipeline:

```
name: Deploy ReactJS to S3

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

    - name: Deploy to S3
      env:
        AWS_ACCESS_KEY_ID: ${{ secrets.AWS_ACCESS_KEY_ID }}
        AWS_SECRET_ACCESS_KEY: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
      run: |
        aws s3 sync frontend/build/ s3://${{ secrets.BUCKET_NAME }}/ --delete
        aws cloudfront create-invalidation --distribution-id ${{ secrets.CLOUDFRONT_DISTRIBUTION_ID }} --paths "/*"
```


Configure Secrets in GitHub:

Add the following secrets in your GitHub repository settings:
AWS_ACCESS_KEY_ID: AWS access key.
AWS_SECRET_ACCESS_KEY: AWS secret key.
BUCKET_NAME: The unique S3 bucket name (e.g., myapp-frontend-20250521).
CLOUDFRONT_DISTRIBUTION_ID: The CloudFront distribution ID (from terraform apply output or AWS Console).





Step 6: Test and Verify

Access the Application:

Use the CloudFront domain name (from outputs.tf) to access the app, e.g., https://d1234567890abcdef.cloudfront.net.
Verify the React app loads and SPA routing works (e.g., navigating to /about loads index.html).
Note: Do not use an IP address, as CloudFront relies on DNS-based routing for its edge locations.


Verify CI/CD:

Push changes to the main branch and monitor the GitHub Actions workflow.
Check the S3 bucket to confirm build/ files are uploaded.
Verify CloudFront invalidation completes to refresh the cache.



Step 7: Handover and Documentation

Share the Repository: Provide access to the GitHub repository containing the ReactJS code, Terraform configurations, and CI/CD workflow.
Document Access:
AWS credentials (stored securely, not in Git).
GitHub repository secrets.
CloudFront domain name (e.g., https://d1234567890abcdef.cloudfront.net) for testing.
S3 bucket name.


Train Team Members:
Explain the Terraform setup, .gitignore, and CI/CD pipeline.
Demonstrate accessing the app via the CloudFront domain and verifying deployments.
Clarify that no custom domain is needed for testing; the CloudFront domain is sufficient.



Troubleshooting

Terraform Errors: Ensure the bucket name is globally unique and AWS CLI is configured. Verify terraform.tfvars is excluded by .gitignore.
CI/CD Failures: Confirm GitHub secrets are set and AWS credentials have S3 and CloudFront permissions.
App Not Loading: Check the S3 website configuration and CloudFront origin settings. Ensure CloudFront invalidation completes.
Routing Issues: Verify the error_document is set to index.html for SPA routing.

Next Steps

Optionally add a custom domain with AWS Route 53 and an SSL certificate via AWS Certificate Manager (not required for testing).
Enable S3 and CloudFront logging for monitoring.
Optimize CloudFront caching settings for performance.

Clean up resource

if you've encountered error when delete S3 bucket, please empty one and retry **terraform destroy**
```
aws s3 rm s3://myapp-frontend-20250521/ --recursive
```
