using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace Entitys.Migrations
{
    public partial class Book120Changed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "CHIEFACCOUNTANT",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BOOK_120",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "CHIEFACCOUNTANT",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BOOK_120",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);
        }
    }
}
