using Microsoft.EntityFrameworkCore.Migrations;

namespace Entitys.Migrations
{
    public partial class ToJornal16StatusColAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "STATUS",
                table: "JOURNAL_176",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "STATUS",
                table: "JOURNAL_16",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "JOURNAL_176");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "JOURNAL_16");
        }
    }
}
