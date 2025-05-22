variable "aws_region" {
  description = "AWS region"
  default     = "us-east-1"
}

variable "repository" {
  description = "repo link"
  default     = "https://github.com/hoale240803/backend-note-book"
}

variable "github_token" {
  description = "GitHub personal access token for Amplify"
  type        = string
  sensitive   = true
}
