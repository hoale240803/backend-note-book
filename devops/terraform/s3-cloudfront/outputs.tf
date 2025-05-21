output "s3_bucket_name" {
  value = aws_s3_bucket.frontend_bucket.bucket
}

output "cloudfront_domain_name" {
  value       = aws_cloudfront_distribution.frontend_distribution.domain_name
  description = "Use this domain (e.g., https://<domain>.cloudfront.net) to access the app for testing"
}

output "cloudfront_distribution_id" {
  description = "The ID of the CloudFront distribution"
  value       = aws_cloudfront_distribution.frontend_distribution.id
}
