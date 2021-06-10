using Microsoft.EntityFrameworkCore.Migrations;

namespace Entitys.Migrations
{
    public partial class SprObjectIdColumnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SPR_OBJECT_ID",
                table: "JOURNAL_176",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SPR_OBJECT_ID",
                table: "JOURNAL_15",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SPR_OBJECT_ID",
                table: "JOURNAL_176");

            migrationBuilder.DropColumn(
                name: "SPR_OBJECT_ID",
                table: "JOURNAL_15");
        }
    }
}
