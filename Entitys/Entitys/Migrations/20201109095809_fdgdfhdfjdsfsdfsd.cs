using Microsoft.EntityFrameworkCore.Migrations;

namespace Entitys.Migrations
{
    public partial class fdgdfhdfjdsfsdfsd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OUTHER_INOUT",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    NAME = table.Column<string>(nullable: true),
                    IS_MAIN_CASHIER = table.Column<bool>(nullable: false),
                    INOUT_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUTHER_INOUT", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OUTHER_INOUT");
        }
    }
}
