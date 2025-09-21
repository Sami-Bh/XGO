locals {
  modified_uuid = substr(replace("${random_uuid.unique_string_uuid.id}", "-", ""), 0, 10)
}
