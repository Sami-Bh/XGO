resource "azurerm_resource_group" "rg-xgo" {
  location = var.resources_region
  name     = var.grp_name
}
