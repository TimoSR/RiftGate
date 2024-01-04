provider "google" {
  project = var.project
  region = var.region
}

provider "google-beta" {
  project = var.project
  region = var.region
}

# We need to c enable the secret manager
# Getting reference to the secret manager
resource "google_project_service" "secret_manager" {
  provider = google-beta
  service = "secretmanager.googleapis.com"

  disable_on_destroy = false
}

resource "google_project_iam_member" "secret_accessor" {
  project = var.project
  role    = "roles/secretmanager.secretAccessor"
  member  = "serviceAccount:153464354706-compute@developer.gserviceaccount.com"
}

# This is used to register the keys for the secrets
# I need to all the secrect variable names here to target them
# Getting the references to secrets
data "google_secret_manager_secret_version" "secrets" {
  for_each = toset(["MONGODB_CONNECTION_STRING","REDIS_CONNECTION_STRING"])
  secret   = each.key
  version  = "latest"
  depends_on = [ google_project_service.secret_manager ]
}

# Creating the Google Cloud Run
resource "google_cloud_run_service" "authentication-service" {
  name     = "auction-service"
  provider = google
  location = var.region

  metadata {
    annotations =  {
        "run.googleapis.com/ingress" = "all"
    }
  }

  template {
    
    spec {

      containers {

        image = var.container_image

        ports {
          container_port = 8080 # Make sure your application listens on port 8080 inside the container.
        }

        # Adding environment variables dynamically
        dynamic "env" {
          for_each = var.env_vars
          content {
            name  = env.key
            value = env.value
          }
        }

        # Dynamicaly adding references to the secrets
        dynamic "env" {
          for_each = data.google_secret_manager_secret_version.secrets
          content {
            name = env.value.secret
            value_from {
              secret_key_ref {
                name = env.value.secret
                key  = "latest"  # Key of the secret in Secret Manager
              }
            }
          }
        }
        
        # Setting the amount of cpu and memory
        resources {
          limits = {
            cpu    = var.cpu
            memory = var.memory
          }
        }
      }   
    }

    # Settings for cloud run scaling
    metadata {
      annotations = {
        # Scaling to zero enabled
        "autoscaling.knative.dev/minScale" = "0"
        # Auto Scaling is max 1 container out of 300
        "autoscaling.knative.dev/maxScale" = "1"
      }
    }
  }
  traffic {
      percent         = 100
      latest_revision = true
  }
}

# # Disabling authentication on microservice for smoke testing 
# resource "google_cloud_run_service_iam_member" "all_users" {
#   location = var.region
#   service  = google_cloud_run_service.x-service.name
#   role     = "roles/run.invoker"
#   member   = "allUsers"
# }

data "google_project" "project" {
  project_id = var.project
}

#Enabling Pub/Sub

resource "google_service_account" "sa" {
  account_id   = "auth-pubsub-inv"  # Shortened to meet the character limit
  display_name = "Authentication Pub/Sub Invoker"

  lifecycle {
    create_before_destroy = true
  }
}

resource "google_cloud_run_service_iam_binding" "binding" {
  location = google_cloud_run_service.authentication-service.location
  service  = google_cloud_run_service.authentication-service.name
  role     = "roles/run.invoker"
  members  = [
    "allUsers",
    "serviceAccount:${google_service_account.sa.email}"
  ]
}

resource "google_project_service_identity" "pubsub_agent" {
  provider = google-beta
  project  = data.google_project.project.project_id
  service  = "pubsub.googleapis.com"
}

resource "google_project_iam_binding" "project_token_creator" {
  project = data.google_project.project.project_id
  role    = "roles/iam.serviceAccountTokenCreator"
  members = ["serviceAccount:${google_project_service_identity.pubsub_agent.email}"]
}