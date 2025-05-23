
# Output
output "beanstalk_url" {
  value       = aws_elastic_beanstalk_environment.react_env.endpoint_url
  description = "Use this URL (e.g., http://<env>.us-east-1.elasticbeanstalk.com) for testing"
}
