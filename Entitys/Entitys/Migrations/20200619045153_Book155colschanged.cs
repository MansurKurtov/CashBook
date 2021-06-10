using Microsoft.EntityFrameworkCore.Migrations;

namespace Entitys.Migrations
{
    public partial class Book155colschanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "COUNTER_CASHIER_ID",
                table: "BOOK_155",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "FROM_175",
                table: "BOOK_155",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "COUNTER_CASHIER_ID",
                table: "BOOK_155");

            migrationBuilder.DropColumn(
                name: "FROM_175",
                table: "BOOK_155");
        }
    }
}
