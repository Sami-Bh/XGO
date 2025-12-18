variable "resources_region" {
  type    = string
  default = "France Central"
}
variable "grp_name" {
  default = "rg-xgo"
}

variable "appRegistrationApi" {
  type    = string
  default = "XGOApi"
}
variable "appRegistrationReact" {
  type    = string
  default = "XGOReact"
}

variable "picturesStorageAccountName" {
  type    = string
  default = "sapictures"
}

variable "picturesComputerVisionName" {
  type    = string
  default = "picturescomputervision"
}

variable "WebApps_service_plan_Name" {
  type    = string
  default = "WebAppsServicePlan"
}

variable "WebApps_Proxy_Name" {
  type    = string
  default = "WebAppsProxy"
}

variable "WebApps_Store_Name" {
  type    = string
  default = "WebAppsStore"
}

variable "WebApps_Storage_Name" {
  type    = string
  default = "WebAppsStorage"
}

variable "Sql_Server_Name" {
  type    = string
  default = "xgodb"
}

variable "Sql_Database_Name" {
  description = "database name"
  type        = string
  default     = "xgodb"
}

variable "sql_admin_username" {
  description = "SQL Server admin username"
  type        = string
  sensitive   = true
}

variable "sql_admin_password" {
  description = "SQL Server admin password"
  type        = string
  sensitive   = true
}

variable "Tenant_Id" {
  description = "Azure Tenant id"
  type        = string
  sensitive   = true
}
