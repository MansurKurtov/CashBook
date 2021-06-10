using Microsoft.EntityFrameworkCore.Migrations;

namespace Entitys.Migrations
{
    public partial class OtherxxTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OUTHER_INOUT");

            migrationBuilder.CreateTable(
                name: "OTHER_INOUT",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    NAME = table.Column<string>(nullable: true),
                    IS_MAIN_CASHIER = table.Column<bool>(nullable: false),
                    INOUT_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTHER_INOUT", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "OTHER_INOUT",
                columns: new[] { "ID", "INOUT_ID", "IS_MAIN_CASHIER", "NAME" },
                values: new object[,]
                {
                    { 1, 1, true, "Омбордан" },
                    { 2, 2, true, "Омборга" },
                    { 3, 2, true, "Жунатма" },
                    { 4, 1, true, "Мадад" },
                    { 5, 1, false, "Кирим" },
                    { 6, 2, false, "Чиким" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OTHER_INOUT");

            migrationBuilder.CreateTable(
                name: "OUTHER_INOUT",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    INOUT_ID = table.Column<int>(nullable: false),
                    IS_MAIN_CASHIER = table.Column<bool>(nullable: false),
                    NAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUTHER_INOUT", x => x.ID);
                });
        }
    }
}
