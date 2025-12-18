// main resource group
data "azurerm_resource_group" "rg-xgo" {
  name = var.grp_name
}

// application registration for the backend api
resource "azuread_application_registration" "XGOApi" {
  display_name     = var.appRegistrationApi
  sign_in_audience = "AzureADMyOrg"

}

// add Application ID URI for xgoapi
resource "azuread_application_identifier_uri" "api_ad" {
  application_id = azuread_application_registration.XGOApi.id
  identifier_uri = "api://${azuread_application_registration.XGOApi.client_id}"
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


// application registration for the front app
resource "azuread_application_registration" "XGOReactApp" {
  display_name     = var.appRegistrationReact
  sign_in_audience = "AzureADMyOrg"
}

// service princpals
resource "azuread_service_principal" "XGOReactApp_service_principal" {
  client_id = azuread_application_registration.XGOReactApp.client_id
}

resource "azuread_service_principal" "XGOApi_service_principal" {
  client_id = azuread_application_registration.XGOApi.client_id
}

// api permission
resource "azuread_service_principal_delegated_permission_grant" "PermisisonGrantForXGOReactApp" {
  resource_service_principal_object_id = azuread_service_principal.XGOApi_service_principal.object_id
  service_principal_object_id          = azuread_service_principal.XGOReactApp_service_principal.object_id
  claim_values                         = ["api.access"]
}

resource "random_uuid" "pictures_Storage_scope_uuid" {}


resource "azurerm_storage_account" "pictures_Storage" {
  account_replication_type = "LRS"
  account_tier             = "Standard"
  location                 = var.resources_region
  resource_group_name      = local.main_resource_group_name
  name                     = module.unique_pictures_storage_accountName.unique_id_string
}

resource "azurerm_cognitive_account" "pictures_Recognizer" {
  name                = module.unique_pictures_computer_vision_name.unique_id_string
  location            = var.resources_region
  resource_group_name = local.main_resource_group_name
  kind                = "ComputerVision"
  sku_name            = "F0"
  identity {
    type = "SystemAssigned"
  }
}
