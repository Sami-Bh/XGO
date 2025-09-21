module "unique_name_or_id_generator_1" {
  source       = "./modules/unique_name_or_id_generator"
  input_string = var.picturesStorageAccountName
}
