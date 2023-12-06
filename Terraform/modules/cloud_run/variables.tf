variable "mongodb_development_db" {
  description = "The name of the secret in Google Secret Manager for MongoDB Development DB"
}

variable "environment" {
  description = "The name of the secret in Google Secret Manager for environment"
}

variable "mongodb_connection_string" {
  description = "The name of the secret in Google Secret Manager for MongoDB Connection String"
}

variable "cpu" {
  description = "CPU allocation for the Cloud Run service"
  type        = string
  default     = "1"
}

variable "memory" {
  description = "Memory allocation for the Cloud Run service"
  type        = string
  default     = "128MiB"
}

variable "location" {
  description = "Location for the Cloud Run service"
  type        = string
  default     = "us-central1"
}