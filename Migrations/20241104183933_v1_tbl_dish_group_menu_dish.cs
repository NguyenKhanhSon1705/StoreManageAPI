using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace StoreManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class v1_tbl_dish_group_menu_dish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dish",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Dish_Name = table.Column<string>(type: "longtext", nullable: false),
                    Unit_Name = table.Column<string>(type: "longtext", nullable: false),
                    Origin_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Selling_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Image = table.Column<string>(type: "longtext", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Create_At = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Is_Hot = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Inventory = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dish", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Menu_Groups_Dish",
                columns: table => new
                {
                    Dish_Id = table.Column<int>(type: "int", nullable: false),
                    Menu_Group_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu_Groups_Dish", x => new { x.Dish_Id, x.Menu_Group_Id });
                    table.ForeignKey(
                        name: "FK_Menu_Groups_Dish_Dish_Dish_Id",
                        column: x => x.Dish_Id,
                        principalTable: "Dish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Menu_Groups_Dish_MenuGroups_Menu_Group_Id",
                        column: x => x.Menu_Group_Id,
                        principalTable: "MenuGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Groups_Dish_Menu_Group_Id",
                table: "Menu_Groups_Dish",
                column: "Menu_Group_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menu_Groups_Dish");

            migrationBuilder.DropTable(
                name: "Dish");
        }
    }
}
