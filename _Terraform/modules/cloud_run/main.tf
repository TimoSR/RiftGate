resource "google_cloud_run_service" "service_x" {
  name     = "service_x"
  location = var.location
  template {
    spec {
      containers {

        # The container should listen for HTTP requests on port 8080 

        image = "docker.io/00tir2009/x_service_dev:latest"

        dynamic "env" {
          for_each = tomap({
            "MONGODB_DEVELOPMENT_DB" = data.google_secret_manager_secret_version.mongodb_development_db.secret_data
            "ENVIRONMENT" = data.google_secret_manager_secret_version.environment.secret_data
            "MONGODB_CONNECTION_STRING" = data.google_secret_manager_secret_version.mongodb_connection_string.secret_data
          })
          
          content {
            name  = env.key
            value = env.value
          }
        }
        resources {
          limits = {
            cpu    = var.cpu
            memory = var.memory
          }
        }
      }
    }
  }

  traffic {
    percent         = 100
    latest_revision = true
  }
}

data "google_secret_manager_secret_version" "environment" {
  secret_id = var.environment
}
data "google_secret_manager_secret_version" "mongodb_development_db" {
  secret_id = var.mongodb_development_db
}

data "google_secret_manager_secret_version" "mongodb_connection_string" {
  secret_id = var.mongodb_connection_string
}