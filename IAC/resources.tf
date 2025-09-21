// main resource group
resource "azurerm_resource_group" "rg-xgo" {
  location = var.resources_region
  name     = var.grp_name
}

// application registration for the backend api
resource "azuread_application_registration" "XGOApi" {
  display_name     = var.appRegistrationApi
  sign_in_audience = "AzureADMyOrg"
}

// uuid used for scope
resource "random_uuid" "XGOApi_scope_uuid" {}

// scope creation
resource "azuread_application_permission_scope" "XGOApi_scope" {
  application_id = azuread_application_registration.XGOApi.id
  scope_id       = random_uuid.XGOApi_scope_uuid.id
  value          = "api.access"

  type                       = "Admin"
  admin_consent_description  = "api.access"
  admin_consent_display_name = "api.access"
}

resource "random_uuid" "pictures_Storage_scope_uuid" {}


resource "azurerm_storage_account" "pictures_Storage" {
  account_replication_type = "LRS"
  account_tier             = "Standard"
  location                 = azurerm_resource_group.rg-xgo.location
  resource_group_name      = azurerm_resource_group.rg-xgo.name
  name                     = module.unique_name_or_id_generator_1.unique_id_string
}
