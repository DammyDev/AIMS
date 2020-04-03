using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectAPI.Migrations
{
    public partial class StatusField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Solution",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ServerInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Databases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Applications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Solution");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ServerInfo");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Databases");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Applications");
        }
    }
}
