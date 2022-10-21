using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace musicCatalogDotNetAPI.Migrations
{
    public partial class CleanupEmails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Song",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 21, 12, 45, 52, 986, DateTimeKind.Local).AddTicks(9391));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 21, 12, 45, 52, 986, DateTimeKind.Local).AddTicks(9425));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Song");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 20, 17, 41, 48, 223, DateTimeKind.Local).AddTicks(3140));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2022, 10, 20, 17, 41, 48, 223, DateTimeKind.Local).AddTicks(3169));
        }
    }
}
