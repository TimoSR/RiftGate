terraform {
  cloud {
    organization = "RiftGate"

    workspaces {
      name = "2_Project-R-Staging"
    }
  }
}