using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class v2_tbl_MenuGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MenuGroups",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "MenuGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MenuGroups_ShopId",
                table: "MenuGroups",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuGroups_Shop_ShopId",
                table: "MenuGroups",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuGroups_Shop_ShopId",
                table: "MenuGroups");

            migrationBuilder.DropIndex(
                name: "IX_MenuGroups_ShopId",
                table: "MenuGroups");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "MenuGroups");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MenuGroups",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);
        }
    }
}
