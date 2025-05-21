# FE
S3 ✅, 
cloudfront ✅, 
beanstalk,
amplify,
ecs



```
resource "aws_s3_bucket" "frontend_bucket" {
  bucket = var.bucket_name
  tags   = { Name = "react-frontend-bucket" }
}
```

Creates an S3 bucket with a name from variable **bucket_name**
Adds a tag for identification
**S3 Bucket Website Configuration**
```
resource "aws_s3_bucket_website_configuration" "frontend_website" {
  bucket = aws_s3_bucket.frontend_bucket.id
  index_document {
    suffix = "index.html"
  }
  error_document {
    key = "index.html" # Redirect errors to index.html for SPA routing
  }
}
```

Configures the S3 bucket to host a static website
Sets index.html as the default document

**S3 Public access block**
```
resource "aws_s3_bucket_public_access_block" "frontend_public_access" {
  bucket                  = aws_s3_bucket.frontend_bucket.id
  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}
```
Restricts all public access to the S3 buckete for security
The bucket will only be accessible through CloudFront

**S3 Bucket Policy**
```
resource "aws_s3_bucket_policy" "frontend_policy" {
  bucket = aws_s3_bucket.frontend_bucket.id
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Principal = {
          AWS = aws_cloudfront_origin_access_identity.oai.iam_arn
        }
        Action   = "s3:GetObject"
        Resource = "${aws_s3_bucket.frontend_bucket.arn}/*"
      }
    ]
  })
}
```
Creates a policy that only allows CloudFront(via Origin Access Identity) to read objects from the bucket
Denies all other access attempts


**CloudFront Origin Access Identity(OAI)**
```
resource "aws_cloudfront_origin_access_identity" "oai" {
  comment = "OAI for React frontend"
}
```
CREATES AN IDENTITY THAT cLOUDfRONT WILL USE TO ACCESS THE PRIVATE S3
This is a secruity best practice


**CloudFront Distributiono**

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

Creates a CloudFront CDN distribution that fronts the S3 bucket
Key features:
- Uses the OAI to access the private S3 bucket
- Serves index.html by default
- Only allows GET and HEAD HTTP methods
- Forces HTTPS redirects
- Set caching TTLs (0s min, 1h default, 24max)
- No geographic restrictions
- Uses CloudFront's free default SSL certificate


2. Comparison of AWS Elastic Beanstalk, Amplify, and ECS for ReactJS Deployment
Below is a comparison of the three services for deploying a ReactJS application, focusing on ease of use, scalability, control, and suitability for your testing scenario. This draws from your interest in straightforward deployment and aligns with your prior use of Terraform and GitHub Actions.

| Feature             | AWS Elastic Beanstalk                                               | AWS Amplify                                              | AWS ECS (with Fargate)                                       |
|---------------------|---------------------------------------------------------------------|----------------------------------------------------------|--------------------------------------------------------------|
| Service Type        | PaaS (Platform as a Service)                                        | BaaS (Backend as a Service)                              | Container Orchestration (IaaS/Serverless with Fargate)       |
| Best For            | Developers wanting managed deployment with some control             | Frontend developers needing quick, serverless deployment | Teams needing containerized apps with fine-grained control   |
| ReactJS Deployment  | Host static files or Node.js server serving React                   | Host static React apps with built-in CI/CD               | Run React in a Docker container (e.g., with Nginx)           |
| Ease of Use         | Moderate: Upload code, Beanstalk handles infrastructure             | High: Simple setup for static sites, Git-based CI/CD     | Moderate to Complex: Requires Docker and cluster setup       |
| Scalability         | Auto-scales with EC2 instances, customizable rules                  | Auto-scales serverlessly via CloudFront/S3               | Auto-scales with Fargate or EC2, highly configurable         |
| Control             | Moderate: Control over EC2 instances, less over infra               | Low: Fully managed, limited infra customization          | High: Full control over containers and networking            |
| Default Domain      | *.elasticbeanstalk.com (e.g., myapp.us-east-1.elasticbeanstalk.com) | *.amplifyapp.com (e.g., d123456.amplifyapp.com)          | ECS with Fargate uses ALB domain (e.g., *.elb.amazonaws.com) |
| Cost                | Pay for EC2 instances, ELB, etc.; no Beanstalk fee                  | Free tier available, pay for usage (S3, CloudFront)      | Pay for Fargate vCPUs/memory or EC2, plus ALB costs          |
| CI/CD Integration   | Supports GitHub Actions, CodePipeline                               | Built-in Git-based CI/CD, seamless with GitHub           | Supports GitHub Actions, CodePipeline, but manual setup      |
| Testing Suitability | Good for testing with Node.js backend, uses default domain          | Ideal for quick testing of static React apps             | Best for containerized testing, more setup needed            |
| Learning Curve      | Moderate: Familiar to .NET developers, some AWS knowledge           | Low: Frontend-focused, minimal AWS knowledge needed      | High: Requires Docker, ECS, and networking knowledge         |
| Terraform Support   | Well-supported, manages EC2, ELB, etc.                              | Limited, as Amplify CLI is preferred                     | Strong, manages ECS clusters, tasks, and ALB                 |


1. AWS Elastic Beanstalk
Elastic Beanstalk can deploy a React app either as a static site (served by a Node.js server) or with a backend. For simplicity, I’ll configure it to serve the static build/ files using a Node.js server.

Terraform Configuration (terraform/beanstalk/main.tf):
```
provider "aws" {
  region = var.aws_region
}

# Elastic Beanstalk Application
resource "aws_elastic_beanstalk_application" "react_app" {
  name        = "react-frontend"
  description = "ReactJS frontend application"
}

# Elastic Beanstalk Environment
resource "aws_elastic_beanstalk_environment" "react_env" {
  application         = aws_elastic_beanstalk_application.react_app.name
  name                = "react-frontend-env"
  solution_stack_name = "64bit Amazon Linux 2 v3.5.1 running Node.js 16"
  setting {
    namespace = "aws:autoscaling:launchconfiguration"
    name      = "InstanceType"
    value     = "t2.micro"
  }
  setting {
    namespace = "aws:elasticbeanstalk:environment"
    name      = "EnvironmentType"
    value     = "SingleInstance" # For testing, single instance
  }
  tags = { Name = "react-frontend-beanstalk" }
}

# Output
output "beanstalk_url" {
  value       = aws_elastic_beanstalk_environment.react_env.endpoint_url
  description = "Use this URL (e.g., http://<env>.us-east-1.elasticbeanstalk.com) for testing"
}
```


# BE
API gateway, lambda, ec2, ecs, ekshow 

# Database
rds, dynamo

# Message brokers
sns sqs