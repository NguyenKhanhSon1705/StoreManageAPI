using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class v2_abortedTableDish_addcolumnShopId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "shop_id",
                table: "AbortedTables",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbortedTables_shop_id",
                table: "AbortedTables",
                column: "shop_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AbortedTables_Shop_shop_id",
                table: "AbortedTables",
                column: "shop_id",
                principalTable: "Shop",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbortedTables_Shop_shop_id",
                table: "AbortedTables");

            migrationBuilder.DropIndex(
                name: "IX_AbortedTables_shop_id",
                table: "AbortedTables");

            migrationBuilder.DropColumn(
                name: "shop_id",
                table: "AbortedTables");
        }
    }
}
