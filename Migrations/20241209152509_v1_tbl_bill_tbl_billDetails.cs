using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace StoreManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class v1_tbl_bill_tbl_billDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(type: "varchar(255)", nullable: true),
                    table_id = table.Column<int>(type: "int", nullable: true),
                    shop_id = table.Column<int>(type: "int", nullable: true),
                    time_start = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    time_end = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    total_money = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    total_quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VAT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    payment_method = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bills_Shop_shop_id",
                        column: x => x.shop_id,
                        principalTable: "Shop",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bills_Tables_table_id",
                        column: x => x.table_id,
                        principalTable: "Tables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bills_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BillDetails",
                columns: table => new
                {
                    bill_id = table.Column<int>(type: "int", nullable: false),
                    dish_id = table.Column<int>(type: "int", nullable: false),
                    selling_price_id = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    notes = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDetails", x => new { x.dish_id, x.bill_id });
                    table.ForeignKey(
                        name: "FK_BillDetails_Bills_bill_id",
                        column: x => x.bill_id,
                        principalTable: "Bills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDetails_DishPriceVersions_selling_price_id",
                        column: x => x.selling_price_id,
                        principalTable: "DishPriceVersions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_BillDetails_Dish_dish_id",
                        column: x => x.dish_id,
                        principalTable: "Dish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_bill_id",
                table: "BillDetails",
                column: "bill_id");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_selling_price_id",
                table: "BillDetails",
                column: "selling_price_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_shop_id",
                table: "Bills",
                column: "shop_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_table_id",
                table: "Bills",
                column: "table_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_user_id",
                table: "Bills",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillDetails");

            migrationBuilder.DropTable(
                name: "Bills");
        }
    }
}
