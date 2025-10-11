using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XGO.Storage.Api.Storage.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedProductName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "StoredItems",
                newName: "ProductExpiryDate");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "StoredItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "StoredItems");

            migrationBuilder.RenameColumn(
                name: "ProductExpiryDate",
                table: "StoredItems",
                newName: "ExpiryDate");
        }
    }
}
