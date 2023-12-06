output "service_url" {
  description = "The URL of the Cloud Run service"
  value       = google_cloud_run_service.default.status[0].url
}

output "service_name" {
  description = "The name of the Cloud Run service"
  value       = google_cloud_run_service.default.name
}

output "service_location" {
  description = "The location of the Cloud Run service"
  value       = google_cloud_run_service.default.location
}
