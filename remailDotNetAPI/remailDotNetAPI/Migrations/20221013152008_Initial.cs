using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace remailDotNetAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    OriginalUserName = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserRole = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "MasterEmail",
                columns: table => new
                {
                    MasterEmailId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterEmail", x => x.MasterEmailId);
                    table.ForeignKey(
                        name: "FK_MasterEmail_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubEmail",
                columns: table => new
                {
                    SubEmailId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    MasterEmailId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubEmail", x => x.SubEmailId);
                    table.ForeignKey(
                        name: "FK_SubEmail_MasterEmail_MasterEmailId",
                        column: x => x.MasterEmailId,
                        principalTable: "MasterEmail",
                        principalColumn: "MasterEmailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "CreatedAt", "OriginalUserName", "Password", "UserName", "UserRole" },
                values: new object[] { 1, new DateTime(2022, 10, 13, 17, 20, 8, 414, DateTimeKind.Local).AddTicks(1208), "testuser1", "testpass1", "testuser1", "BasicUser" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "CreatedAt", "OriginalUserName", "Password", "UserName", "UserRole" },
                values: new object[] { 2, new DateTime(2022, 10, 13, 17, 20, 8, 414, DateTimeKind.Local).AddTicks(1241), "testuser2", "testpass2", "testuser2", "BasicUser" });

            migrationBuilder.CreateIndex(
                name: "IX_MasterEmail_UserId",
                table: "MasterEmail",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubEmail_MasterEmailId",
                table: "SubEmail",
                column: "MasterEmailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubEmail");

            migrationBuilder.DropTable(
                name: "MasterEmail");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
