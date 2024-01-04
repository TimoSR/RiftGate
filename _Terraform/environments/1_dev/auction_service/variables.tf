variable "env_vars" {
  description = "Environment variables for the Cloud Run service"
  type = map(string)
  default = {    

    ########################################################################################
    # HOST_URL should be URL without ending '/'
    ########################################################################################
    "HOST_URL"                    = ""
    "SERVICE_NAME"                = "auction-service"
    "ENVIRONMENT"                 = "Production"
    "GOOGLE_CLOUD_PROJECT"        = "bachelor-project-407111"
    "JWT_KEY"                     = "NzQ5OTU4YzEtNmFhZi00YmFkLWIxNWMtYjY4YzQwN2I3ZDA5"
    "JWT_AUDIENCE"                = "Riftgate-Customer"

    ########################################################################################
    
  }
}

variable "project" {
  default     = "bachelor-project-407111"
}

variable "region" {
  description = "The region where the Cloud Run service will be deployed."
  default     = "europe-west1" # Replace with your preferred region.
}

variable "container_image" {
  description = "The container image to deploy."
  default     = "docker.io/00tir2009/auction_service_dev:latest" # Replace with your container image.
}

variable "cpu" {
  description = "CPU allocation for the Cloud Run service"
  type        = string
  default     = "1"
}

variable "memory" {
  description = "Memory allocation for the Cloud Run service"
  type        = string
  default     = "512Mi"
}