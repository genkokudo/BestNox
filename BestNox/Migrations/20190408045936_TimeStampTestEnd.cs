using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BestNox.Migrations
{
    public partial class TimeStampTestEnd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "QaDatas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "QaDatas",
                nullable: true);
        }
    }
}
