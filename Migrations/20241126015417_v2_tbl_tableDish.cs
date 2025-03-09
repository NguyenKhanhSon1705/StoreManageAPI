using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class v2_tbl_tableDish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "TableDishs",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "TableDishs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "notes",
                table: "TableDishs");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "TableDishs");
        }
    }
}
