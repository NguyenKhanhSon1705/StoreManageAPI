using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class v4_bill_changetype_bill_code : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "bill_code",
                table: "Bills",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "bill_code",
                table: "Bills",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);
        }
    }
}
