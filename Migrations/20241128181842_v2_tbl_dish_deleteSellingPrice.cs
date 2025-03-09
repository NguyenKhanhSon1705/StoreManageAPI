using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class v2_tbl_dish_deleteSellingPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Selling_Price",
                table: "Dish");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Selling_Price",
                table: "Dish",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
