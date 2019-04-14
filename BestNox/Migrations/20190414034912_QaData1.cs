using Microsoft.EntityFrameworkCore.Migrations;

namespace BestNox.Migrations
{
    public partial class QaData1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsPublic",
                table: "QaDatas",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsPublic",
                table: "QaDatas",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
