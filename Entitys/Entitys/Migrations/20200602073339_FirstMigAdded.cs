using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace Entitys.Migrations
{
    public partial class FirstMigAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SUP_ACCOUNTANT",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    FIO = table.Column<string>(nullable: true),
                    BANK_CODE = table.Column<int>(nullable: false),
                    CREATED_USER_ID = table.Column<int>(nullable: false),
                    CREATE_DATE = table.Column<DateTime>(nullable: false),
                    UPDATED_USER_ID = table.Column<int>(nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUP_ACCOUNTANT", x => x.ID);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropTable(
                name: "SUP_ACCOUNTANT");
            
        }
    }
}
