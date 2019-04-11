using Microsoft.EntityFrameworkCore.Migrations;

namespace BestNox.Migrations
{
    public partial class UploadFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "UploadFiles",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "TmpFilename",
                table: "UploadFiles",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TmpFilename",
                table: "UploadFiles");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "UploadFiles",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
