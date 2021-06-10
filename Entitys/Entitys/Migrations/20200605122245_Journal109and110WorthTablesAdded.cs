using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace Entitys.Migrations
{
    public partial class Journal109and110WorthTablesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JOURNAL_109_WORTH",
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
                    SYMBOL_KOD = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOURNAL_109_WORTH", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JOURNAL_110_WORTH",
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
                    SYMBOL_KOD = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOURNAL_110_WORTH", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JOURNAL_109_WORTH");

            migrationBuilder.DropTable(
                name: "JOURNAL_110_WORTH");
        }
    }
}
