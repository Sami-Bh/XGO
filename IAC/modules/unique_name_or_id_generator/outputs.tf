output "unique_id_string" {
  value = "${var.input_string}${local.modified_uuid}"
}
