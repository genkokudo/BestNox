using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BestNox.Migrations
{
    public partial class TableAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelativeNo",
                table: "SystemParameters");

            migrationBuilder.CreateTable(
                name: "DailyRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(type: "text", maxLength: 50, nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QaDatas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "text", maxLength: 50, nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: true),
                    IsSolved = table.Column<bool>(nullable: false),
                    RelativeNo = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    IsPublic = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QaDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: false),
                    Filename = table.Column<string>(nullable: false),
                    IsPublic = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyRecords_CreatedBy",
                table: "DailyRecords",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QaDatas_CreatedBy",
                table: "QaDatas",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_CreatedBy",
                table: "UploadFiles",
                column: "CreatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyRecords");

            migrationBuilder.DropTable(
                name: "QaDatas");

            migrationBuilder.DropTable(
                name: "UploadFiles");

            migrationBuilder.AddColumn<int>(
                name: "RelativeNo",
                table: "SystemParameters",
                nullable: false,
                defaultValue: 0);
        }
    }
}
