using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BestNox.Migrations
{
    public partial class TimeStampTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "QaDatas",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "QaDatas");
        }
    }
}
