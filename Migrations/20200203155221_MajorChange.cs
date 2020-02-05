using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectAPI.Migrations
{
    public partial class MajorChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HostName",
                table: "ServerInfo");

            migrationBuilder.DropColumn(
                name: "ConnectionString",
                table: "Databases");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "ServerInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatingSystem",
                table: "ServerInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ServerInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Databases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Databases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "ServerInfo");

            migrationBuilder.DropColumn(
                name: "OperatingSystem",
                table: "ServerInfo");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ServerInfo");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Databases");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Databases");

            migrationBuilder.AddColumn<string>(
                name: "HostName",
                table: "ServerInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConnectionString",
                table: "Databases",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
