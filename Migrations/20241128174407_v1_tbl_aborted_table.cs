using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace StoreManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class v1_tbl_aborted_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbortedTables",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(type: "varchar(255)", nullable: true),
                    table_id = table.Column<int>(type: "int", nullable: true),
                    reason_abort = table.Column<string>(type: "longtext", nullable: true),
                    total_moneny = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    total_quantity_dish = table.Column<int>(type: "int", nullable: true),
                    create_table_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    aborted_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbortedTables", x => x.id);
                    table.ForeignKey(
                        name: "FK_AbortedTables_Tables_table_id",
                        column: x => x.table_id,
                        principalTable: "Tables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AbortedTables_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AbortedTables_table_id",
                table: "AbortedTables",
                column: "table_id");

            migrationBuilder.CreateIndex(
                name: "IX_AbortedTables_user_id",
                table: "AbortedTables",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbortedTables");
        }
    }
}
