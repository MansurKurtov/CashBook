using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entitys.Migrations
{
    public partial class To176Date16ColAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DATE_16",
                table: "JOURNAL_176",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DATE_16",
                table: "JOURNAL_176");
        }
    }
}
