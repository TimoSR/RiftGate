terraform {
  cloud {
    organization = "RiftGate"

    workspaces {
      name = "3_Project-R-Prod"
    }
  }
}