locals {
  connection_String_name  = "AZURE_SQL_CONNECTIONSTRING"
  connection_String_Type  = "SQLAzure"
  connection_String_value = "Server=tcp:${data.azurerm_mssql_server.xgo_sql_server.fully_qualified_domain_name},1433;Initial Catalog=${var.Sql_Database_Name};Persist Security Info=False;User ID=${var.sql_admin_username};Password=${var.sql_admin_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}

locals {
  web_apps = {
    storeWebApp = {
      name                    = var.WebApps_Store_Name,
      route_path              = "/Store",
      cluster_id              = "Store_Cluster",
      connection_String_name  = local.connection_String_name,
      connection_String_Type  = local.connection_String_Type
      connection_String_value = local.connection_String_value
    }
    storageWebApp = {
      name                    = var.WebApps_Storage_Name,
      route_path              = "/Storage",
      cluster_id              = "Storage_Cluster"
      connection_String_name  = local.connection_String_name,
      connection_String_Type  = local.connection_String_Type
      connection_String_value = local.connection_String_value
    }
    # proxy         = { name = "ProxyWebApp" }
  }
}
