using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace Entitys.Migrations
{
    public partial class AuthRTokenReAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AUTH_USER_RTOKENS",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    USER_ID = table.Column<int>(nullable: false),
                    REFRESH_TOKEN = table.Column<string>(nullable: true),
                    CLIENT_ID = table.Column<string>(nullable: true),
                    CREATED = table.Column<DateTime>(nullable: false),
                    UPDATED = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUTH_USER_RTOKENS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AURTOKENSR_ID",
                        column: x => x.USER_ID,
                        principalTable: "auth_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AU_RTOKENSR_ID",
                table: "AUTH_USER_RTOKENS",
                column: "USER_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUTH_USER_RTOKENS");
        }
    }
}
