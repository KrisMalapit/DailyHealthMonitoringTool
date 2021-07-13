using Microsoft.EntityFrameworkCore.Migrations;

namespace ScreeningTool.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Q21",
                table: "ScreenLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Q21",
                table: "ScreenLogs");
        }
    }
}
