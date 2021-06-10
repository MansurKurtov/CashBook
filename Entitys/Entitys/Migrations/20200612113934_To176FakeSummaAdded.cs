using Microsoft.EntityFrameworkCore.Migrations;

namespace Entitys.Migrations
{
    public partial class To176FakeSummaAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FAKE_SUMMA",
                table: "JOURNAL_176",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FAKE_SUMMA",
                table: "JOURNAL_176");
        }
    }
}
