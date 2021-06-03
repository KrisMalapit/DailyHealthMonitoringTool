using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScreeningTool.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Vaccinated",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstDose",
                table: "Employees",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SecondDose",
                table: "Employees",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstDose",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SecondDose",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "Vaccinated",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
