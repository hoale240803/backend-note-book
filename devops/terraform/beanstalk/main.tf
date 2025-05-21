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
