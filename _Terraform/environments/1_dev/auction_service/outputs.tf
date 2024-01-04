output "service_url" {
  value = google_cloud_run_service.authentication-service.status[0].url
}