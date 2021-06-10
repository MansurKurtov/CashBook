using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace Entitys.Migrations
{
    public partial class AuthTablesAutoIncrementRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "EVENT_HISTORY",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_users",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "AUTH_USER_RTOKENS",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "AUTH_USER_ROLES",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_user_permissions",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_uielements",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_structure_permissins",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_roles",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_role_permissions",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_permissions",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_moduless",
                nullable: true,
                oldClrType: typeof(int))
                .OldAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "EVENT_HISTORY",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_users",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "AUTH_USER_RTOKENS",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "AUTH_USER_ROLES",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_user_permissions",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_uielements",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_structure_permissins",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_roles",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_role_permissions",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_permissions",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "auth_moduless",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);
        }
    }
}
