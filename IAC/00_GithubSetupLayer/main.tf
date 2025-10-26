// main resource group
resource "azurerm_resource_group" "rg-xgo" {
  location = var.resources_region
  name     = var.grp_name
}

resource "azurerm_storage_account" "pictures_Storage" {
  account_replication_type = "LRS"
  account_tier             = "Standard"
  location                 = var.resources_region
  resource_group_name      = local.main_resource_group_name
  #   name                     = module.unique_pictures_storage_accountName.unique_id_string
  name                            = var.picturesStorageAccountName
  allow_nested_items_to_be_public = false

}

# resource "azurerm_security_center_storage_defender" "pictures_Storage_Defender" {
#   storage_account_id = azurerm_storage_account.pictures_Storage.id
# }

resource "azurerm_user_assigned_identity" "GithubManagedIdentity" {
  location            = azurerm_resource_group.rg-xgo.location
  name                = "GithubManagedIdentity"
  resource_group_name = azurerm_resource_group.rg-xgo.name
}

resource "azurerm_federated_identity_credential" "PullRequestFederatedIdentity" {
  name                = "xgo_federated_identity"
  resource_group_name = azurerm_resource_group.rg-xgo.name
  audience            = ["api://AzureADTokenExchange"]
  issuer              = "https://token.actions.githubusercontent.com"
  parent_id           = azurerm_user_assigned_identity.GithubManagedIdentity.id
  subject             = "repo:Sami-Bh/XGO:pull_request"
}

data "azurerm_subscription" "primary" {
}

resource "azurerm_role_assignment" "GithubManagedIdentityRoleAssignment" {
  scope                = data.azurerm_subscription.primary.id
  role_definition_name = "Contributor"
  principal_id         = azurerm_user_assigned_identity.GithubManagedIdentity.principal_id
}

resource "azurerm_storage_container" "TerraformStateBlob" {
  name                 = "terraformstateblob"
  storage_account_name = azurerm_storage_account.pictures_Storage.name

  container_access_type = "private"
}
