using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XGO.Storage.Api.Storage.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addAcknowledgedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExpiracyAcknowledged",
                table: "StoredItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExpiracyAcknowledged",
                table: "StoredItems");
        }
    }
}
