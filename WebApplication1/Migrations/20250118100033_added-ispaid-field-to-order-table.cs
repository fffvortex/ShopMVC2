using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopMVC2.Migrations
{
    /// <inheritdoc />
    public partial class addedispaidfieldtoordertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Order",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Order");
        }
    }
}
