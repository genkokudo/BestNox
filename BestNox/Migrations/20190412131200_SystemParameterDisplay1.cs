using Microsoft.EntityFrameworkCore.Migrations;

namespace BestNox.Migrations
{
    public partial class SystemParameterDisplay1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Display",
                table: "SystemParameters",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Display",
                table: "SystemParameters");
        }
    }
}
