using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class v1_tbl_tableDish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TableDishs",
                columns: table => new
                {
                    tableId = table.Column<int>(type: "int", nullable: false),
                    dishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableDishs", x => new { x.dishId, x.tableId });
                    table.ForeignKey(
                        name: "FK_TableDishs_Dish_dishId",
                        column: x => x.dishId,
                        principalTable: "Dish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableDishs_Tables_tableId",
                        column: x => x.tableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TableDishs_tableId",
                table: "TableDishs",
                column: "tableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableDishs");
        }
    }
}
