// database
data "azurerm_mssql_server" "xgo_sql_server" {
  name                = var.Sql_Server_Name
  resource_group_name = var.grp_name
}

data "azurerm_mssql_database" "xgo_database" {
  name      = var.Sql_Database_Name
  server_id = data.azurerm_mssql_server.xgo_sql_server.id

}

resource "azurerm_service_plan" "WebApps_service_plan" {
  name                = var.WebApps_service_plan_Name
  resource_group_name = data.azurerm_resource_group.rg-xgo.name
  location            = data.azurerm_resource_group.rg-xgo.location
  sku_name            = "F1"
  os_type             = "Windows"
}

resource "azurerm_windows_web_app" "WebApps" {
  for_each            = local.web_apps
  name                = each.value.name
  resource_group_name = data.azurerm_resource_group.rg-xgo.name
  location            = data.azurerm_resource_group.rg-xgo.location
  service_plan_id     = azurerm_service_plan.WebApps_service_plan.id

  site_config {
    always_on = false
    application_stack {
      dotnet_version = "v8.0"
    }
  }
  connection_string {
    name  = each.value.connection_String_name
    type  = each.value.connection_String_Type
    value = each.value.connection_String_value
  }
}

resource "azurerm_windows_web_app" "ProxyWebApp" {
  name                = var.WebApps_Proxy_Name
  resource_group_name = data.azurerm_resource_group.rg-xgo.name
  location            = data.azurerm_resource_group.rg-xgo.location
  service_plan_id     = azurerm_service_plan.WebApps_service_plan.id
  depends_on          = [azuread_application_registration.XGOApi]
  site_config {
    always_on = false
    application_stack {
      dotnet_version = "v8.0"
    }
    # cors { allowed_origins = ["https://${azurerm_windows_web_app.ProxyWebApp.default_hostname}"] }
  }
  app_settings = {
    ReverseProxy = jsonencode({
      Routes = {
        for key, config in local.web_apps :
        "route_${key}" => merge(
          {
            ClusterId = config.cluster_id
            Match = {
              Path = "${config.route_path}/{**catch-all}"
            }
            Transforms = [
              { PathPattern = "{**catch-all}" }
            ]
            AuthorizationPolicy = "customPolicy"
          },
        )
      }

      Clusters = {
        for key, config in local.web_apps :
        config.cluster_id => {
          Destinations = {
            destination1 = {
              Address = "https://${azurerm_windows_web_app.WebApps[key].default_hostname}"
            }
          }
        }
      }
    }),
    AzureAd = jsonencode({
      Instance = "https://login.microsoftonline.com/"
      TenantId = var.Tenant_Id
      ClientId = azuread_application_registration.XGOApi.client_id
    })
  }
}


resource "azapi_update_resource" "WebApps" {
  type        = "Microsoft.Web/sites@2024-11-01"
  for_each    = local.web_apps
  depends_on  = [azurerm_windows_web_app.ProxyWebApp]
  resource_id = azurerm_windows_web_app.ProxyWebApp.id
  body = {
    properties = {
      siteConfig = {
        cors = {
          allowedOrigins = ["https://${azurerm_windows_web_app.ProxyWebApp.default_hostname}"]
        }
      }
    }
  }
}

# the block below is useless

# // add cors so storage and store can only be accessed with proxy
# resource "azapi_update_resource" "WebApps" {
#   type        = "Microsoft.Web/sites@2024-11-01"
#   for_each    = local.web_apps
#   depends_on  = [azurerm_windows_web_app.WebApps, azurerm_windows_web_app.ProxyWebApp]
#   resource_id = azurerm_windows_web_app.WebApps[each.key].id
#   body = {
#     properties = {
#       siteConfig = {
#         cors = {
#           allowedOrigins = ["https://${azurerm_windows_web_app.ProxyWebApp.default_hostname}"]
#         }
#       }
#     }
#   }
#   # Capture the response
#   response_export_values = ["*"]
# }
# # Output to see what was actually set
# output "cors_debug" {
#   value = {
#     for k, v in azapi_update_resource.WebApps : k => {
#       resource_id = v.resource_id
#       output      = v.output
#     }
#   }
#   sensitive = false
# }

