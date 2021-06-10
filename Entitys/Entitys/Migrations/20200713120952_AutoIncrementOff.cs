using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace Entitys.Migrations
{
    public partial class AutoIncrementOff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "SUP_ACCOUNTANT",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_176",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_175",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_16",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_15",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_123_FIO",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_123_CONTENT",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_123",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_111",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_110_WORTH",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_110_VAL",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_110",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_109_WORTH",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_109_VAL",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_109",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "COUNTER_CASHIER",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "COLLECTOR",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BOOK_171",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BOOK_155",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BOOK_141A",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BOOK_141",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BOOK_121",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "SUP_ACCOUNTANT",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_176",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_175",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_16",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_15",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_123_FIO",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_123_CONTENT",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_123",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_111",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_110_WORTH",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_110_VAL",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_110",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_109_WORTH",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_109_VAL",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "JOURNAL_109",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "COUNTER_CASHIER",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "COLLECTOR",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BOOK_171",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BOOK_155",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BOOK_141A",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BOOK_141",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "BOOK_121",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);
        }
    }
}
