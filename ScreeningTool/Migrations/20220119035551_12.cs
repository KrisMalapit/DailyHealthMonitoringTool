using Microsoft.EntityFrameworkCore.Migrations;

namespace ScreeningTool.Migrations
{
    public partial class _12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Q22",
                table: "ScreenLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Q22",
                table: "ScreenLogs");
        }
    }
}
