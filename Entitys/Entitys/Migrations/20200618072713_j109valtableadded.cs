using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace Entitys.Migrations
{
    public partial class j109valtableadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FIO",
                table: "JOURNAL_110_WORTH",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FIO",
                table: "JOURNAL_110",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FIO",
                table: "JOURNAL_109_WORTH",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JOURNAL_109_VAL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    SYMBOL_NAME = table.Column<string>(nullable: true),
                    COUNT = table.Column<int>(nullable: false),
                    SUMMA = table.Column<double>(nullable: false),
                    USER_ID = table.Column<int>(nullable: false),
                    DATE = table.Column<DateTime>(nullable: false),
                    BANK_KOD = table.Column<int>(nullable: false),
                    SYMBOL_KOD = table.Column<int>(nullable: false),
                    FIO = table.Column<string>(nullable: true),
                    VALUT_KOD = table.Column<int>(nullable: false),
                    VALUT_NAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOURNAL_109_VAL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JOURNAL_110_VAL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    SYMBOL_NAME = table.Column<string>(nullable: true),
                    COUNT = table.Column<int>(nullable: false),
                    SUMMA = table.Column<double>(nullable: false),
                    USER_ID = table.Column<int>(nullable: false),
                    DATE = table.Column<DateTime>(nullable: false),
                    BANK_KOD = table.Column<int>(nullable: false),
                    SYMBOL_KOD = table.Column<int>(nullable: false),
                    FIO = table.Column<string>(nullable: true),
                    VALUT_KOD = table.Column<int>(nullable: false),
                    VALUT_NAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOURNAL_110_VAL", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JOURNAL_109_VAL");

            migrationBuilder.DropTable(
                name: "JOURNAL_110_VAL");

            migrationBuilder.DropColumn(
                name: "FIO",
                table: "JOURNAL_110_WORTH");

            migrationBuilder.DropColumn(
                name: "FIO",
                table: "JOURNAL_110");

            migrationBuilder.DropColumn(
                name: "FIO",
                table: "JOURNAL_109_WORTH");
        }
    }
}
