using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XGO.Storage.Api.Storage.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_expiryDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "StoredItems",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "StoredItems");
        }
    }
}
