using Microsoft.EntityFrameworkCore.Migrations;

namespace Entitys.Migrations
{
    public partial class J176DecimalToDoubleChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<double>(
            //    name: "EXCESS_SUMMA",
            //    table: "JOURNAL_176",
            //    nullable: false,
            //    oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "EXCESS_SUMMA",
                table: "JOURNAL_176",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
