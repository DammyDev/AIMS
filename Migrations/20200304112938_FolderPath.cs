using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AIMS.Migrations
{
    public partial class FolderPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDeployed",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "FullPath",
                table: "Applications");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeployed",
                table: "Application_ServerInfo",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FolderPath",
                table: "Application_ServerInfo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDeployed",
                table: "Application_ServerInfo");

            migrationBuilder.DropColumn(
                name: "FolderPath",
                table: "Application_ServerInfo");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeployed",
                table: "Applications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FullPath",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
