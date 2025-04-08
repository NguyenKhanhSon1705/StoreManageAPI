using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace StoreManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class v1_tbl_transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    transaction_id = table.Column<long>(type: "bigint", nullable: true),
                    bill_id = table.Column<int>(type: "int", nullable: true),
                    total_money = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    payment_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    status_name = table.Column<string>(type: "longtext", nullable: true),
                    payment_method = table.Column<string>(type: "longtext", nullable: true),
                    bank_code = table.Column<string>(type: "longtext", nullable: true),
                    message = table.Column<string>(type: "longtext", nullable: true),
                    decription = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Bills_bill_id",
                        column: x => x.bill_id,
                        principalTable: "Bills",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_bill_id",
                table: "Transactions",
                column: "bill_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
