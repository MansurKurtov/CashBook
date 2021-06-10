using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace Entitys.Migrations
{
    public partial class SomeTablesChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "TYPE_CASHIERS",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ref_structures",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ref_regions",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ref_districts",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ref_countries",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "MODIFIED_BOOK_SALDOS",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "INOUT",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "ENT_OPERATION_155",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "TYPE_CASHIERS",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ref_structures",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ref_regions",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ref_districts",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ref_countries",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "MODIFIED_BOOK_SALDOS",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "INOUT",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "ENT_OPERATION_155",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);
        }
    }
}
