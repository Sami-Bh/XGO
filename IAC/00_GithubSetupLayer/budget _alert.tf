resource "azurerm_monitor_action_group" "example" {
  name                = "budget monitor action"
  resource_group_name = azurerm_resource_group.rg-xgo.name
  short_name          = "mon action"
}

resource "azurerm_consumption_budget_resource_group" "example" {
  name              = "total budget"
  resource_group_id = azurerm_resource_group.rg-xgo.id

  amount     = 2
  time_grain = "Monthly"

  time_period {
    start_date = "2025-10-01T00:00:00Z"
    end_date   = "2027-10-01T00:00:00Z"
  }

  filter {
    dimension {
      name = "ResourceId"
      values = [
        azurerm_monitor_action_group.example.id,
      ]
    }
  }

  notification {
    enabled        = true
    threshold      = 90.0
    operator       = "EqualTo"
    threshold_type = "Forecasted"

    contact_emails = [
      local.email_sami,
      local.email_rim,
    ]

    contact_groups = [
      azurerm_monitor_action_group.example.id,
    ]

    contact_roles = [
      "Owner",
    ]
  }

  notification {
    enabled   = true
    threshold = 100.0
    operator  = "GreaterThan"

    contact_emails = [
      local.email_sami,
      local.email_rim,
    ]
  }
}
