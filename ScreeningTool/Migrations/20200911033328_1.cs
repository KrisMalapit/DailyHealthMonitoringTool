using Microsoft.EntityFrameworkCore.Migrations;

namespace ScreeningTool.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Q16",
                table: "ScreenLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Q17",
                table: "ScreenLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Q18",
                table: "ScreenLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Q19",
                table: "ScreenLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Q16",
                table: "ScreenLogs");

            migrationBuilder.DropColumn(
                name: "Q17",
                table: "ScreenLogs");

            migrationBuilder.DropColumn(
                name: "Q18",
                table: "ScreenLogs");

            migrationBuilder.DropColumn(
                name: "Q19",
                table: "ScreenLogs");
        }
    }
}
