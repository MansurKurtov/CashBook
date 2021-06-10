using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace Entitys.Migrations
{
    public partial class Journal123sTablesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JOURNAL_123",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    DATE = table.Column<DateTime>(type: "date", nullable: false),
                    USER_ID = table.Column<int>(nullable: false),
                    SYSTEM_DATE = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOURNAL_123", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JOURNAL_123_CONTENT",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    USER_ID = table.Column<int>(nullable: false),
                    SYSTEM_DATE = table.Column<DateTime>(nullable: false),
                    BANK_KOD = table.Column<int>(nullable: false),
                    NAME = table.Column<string>(nullable: true),
                    COUNT = table.Column<int>(nullable: false),
                    VALUE = table.Column<string>(nullable: true),
                    SUMMA = table.Column<double>(nullable: false),
                    TARGET = table.Column<string>(nullable: true),
                    JOURNAL_123_ID = table.Column<int>(nullable: false),
                    FIO = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOURNAL_123_CONTENT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JOURNAL_123_FIO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    USER_ID = table.Column<int>(nullable: false),
                    JOURNAL_123_ID = table.Column<int>(nullable: false),
                    FIO = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOURNAL_123_FIO", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JOURNAL_123");

            migrationBuilder.DropTable(
                name: "JOURNAL_123_CONTENT");

            migrationBuilder.DropTable(
                name: "JOURNAL_123_FIO");
        }
    }
}
