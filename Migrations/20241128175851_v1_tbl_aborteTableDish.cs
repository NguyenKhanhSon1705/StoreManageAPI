using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class v1_tbl_aborteTableDish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbortedTablesDish",
                columns: table => new
                {
                    aborted_table_id = table.Column<int>(type: "int", nullable: false),
                    dish_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    selling_price_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbortedTablesDish", x => new { x.dish_id, x.aborted_table_id });
                    table.ForeignKey(
                        name: "FK_AbortedTablesDish_AbortedTables_aborted_table_id",
                        column: x => x.aborted_table_id,
                        principalTable: "AbortedTables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbortedTablesDish_DishPriceVersions_selling_price_id",
                        column: x => x.selling_price_id,
                        principalTable: "DishPriceVersions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_AbortedTablesDish_Dish_dish_id",
                        column: x => x.dish_id,
                        principalTable: "Dish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AbortedTablesDish_aborted_table_id",
                table: "AbortedTablesDish",
                column: "aborted_table_id");

            migrationBuilder.CreateIndex(
                name: "IX_AbortedTablesDish_selling_price_id",
                table: "AbortedTablesDish",
                column: "selling_price_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbortedTablesDish");
        }
    }
}
