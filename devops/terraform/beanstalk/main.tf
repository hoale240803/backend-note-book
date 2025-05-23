provider "aws" {
  region = var.aws_region
}

resource "aws_s3_bucket" "react_artifacts" {
  bucket = "react-frontend-artifacts"
}

resource "aws_s3_object" "app_zip" {
  bucket = aws_s3_bucket.react_artifacts.bucket
  key    = "app.zip"
  source = "C:/Workspace/backend-note-book/devops/front-end/app.zip"
}

resource "aws_elastic_beanstalk_application" "react_app" {
  name        = "react-frontend"
  description = "ReactJS frontend application"
}

resource "aws_elastic_beanstalk_application_version" "react_version" {
  application = aws_elastic_beanstalk_application.react_app.name
  name        = "react-frontend-v1"
  bucket      = aws_s3_bucket.react_artifacts.bucket
  key         = aws_s3_object.app_zip.key
}

resource "aws_elastic_beanstalk_environment" "react_env" {
  application         = aws_elastic_beanstalk_application.react_app.name
  name                = "react-frontend-env"
  solution_stack_name = "64bit Amazon Linux 2023 v6.5.2 running Node.js 22" # Verify stack name
  
  version_label       = aws_elastic_beanstalk_application_version.react_version.name

  setting {
    namespace = "aws:autoscaling:launchconfiguration"
    name      = "InstanceType"
    value     = "t2.micro"
  }
  setting {
    namespace = "aws:elasticbeanstalk:environment"
    name      = "EnvironmentType"
    value     = "SingleInstance"
  }
  setting {
    namespace = "aws:elasticbeanstalk:application:environment"
    name      = "PORT"
    value     = "3000"
  }
  tags = { Name = "react-frontend-beanstalk" }
}

output "environment_url" {
  value       = aws_elastic_beanstalk_environment.react_env.cname
  description = "URL of the Elastic Beanstalk environment"
}