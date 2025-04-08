using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace StoreManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class v1_tbl_dishPriceVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DishPriceVersions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    dish_id = table.Column<int>(type: "int", nullable: true),
                    price_version = table.Column<string>(type: "longtext", nullable: true),
                    selling_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    update_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    status = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishPriceVersions", x => x.id);
                    table.ForeignKey(
                        name: "FK_DishPriceVersions_Dish_dish_id",
                        column: x => x.dish_id,
                        principalTable: "Dish",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DishPriceVersions_dish_id",
                table: "DishPriceVersions",
                column: "dish_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishPriceVersions");
        }
    }
}
