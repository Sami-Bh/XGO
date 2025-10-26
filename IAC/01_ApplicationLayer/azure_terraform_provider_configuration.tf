terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=4.0.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "rg-xgo"
    storage_account_name = "xgostorageaccount9260"
    container_name       = "terraformstateblob"
    key                  = "terraform.tfstate"
  }
  required_version = ">= 1.1.0"
}

provider "azurerm" {
  features {}
}
