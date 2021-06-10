using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entitys.Migrations
{
    public partial class datecolumnchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DATE_16",
                table: "JOURNAL_176",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DATE_16",
                table: "JOURNAL_176",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
