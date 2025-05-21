# FE
S3, cloudfront,...


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


# BE
API gateway, lambda, ec2, ecs, eks

# Database
rds, dynamo

# Message brokers
sns sqs