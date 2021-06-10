using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entitys.Migrations
{
    public partial class Journal16AcceptDateColAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ACCEPT_DATE",
                table: "JOURNAL_16",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ACCEPT_DATE",
                table: "JOURNAL_16");
        }
    }
}
