using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entitys.Migrations
{
    public partial class Journal10AddedNewColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DATE",
                table: "JOURNAL_110_WORTH",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DATE",
                table: "JOURNAL_110_VAL",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DATE",
                table: "JOURNAL_109_WORTH",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DATE",
                table: "JOURNAL_109_VAL",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DATE",
                table: "JOURNAL_110_WORTH",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DATE",
                table: "JOURNAL_110_VAL",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DATE",
                table: "JOURNAL_109_WORTH",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DATE",
                table: "JOURNAL_109_VAL",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
