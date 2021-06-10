using Microsoft.EntityFrameworkCore.Migrations;

namespace Entitys.Migrations
{
    public partial class Book155ColChangedToNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "COUNTER_CASHIER_ID",
                table: "BOOK_155",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "COUNTER_CASHIER_ID",
                table: "BOOK_155",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
