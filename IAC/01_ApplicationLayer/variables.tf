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
