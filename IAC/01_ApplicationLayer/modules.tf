module "unique_pictures_storage_accountName" {
  source       = "../modules/unique_name_or_id_generator"
  input_string = var.picturesStorageAccountName
}

module "unique_pictures_computer_vision_name" {
  source       = "../modules/unique_name_or_id_generator"
  input_string = var.picturesComputerVisionName
}
