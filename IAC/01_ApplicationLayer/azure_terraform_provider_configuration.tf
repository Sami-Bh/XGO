terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.10"
    }
    azapi = {
      source = "azure/azapi"
    }
  }

  backend "azurerm" {
    resource_group_name  = "rg-xgo"
    storage_account_name = "xgostorageaccount9260"
    container_name       = "terraformstateblob"
    key                  = "terraform.tfstate"
  }
  required_version = ">= 1.14.0"
}

provider "azurerm" {
  features {}
}
provider "azapi" {
}
