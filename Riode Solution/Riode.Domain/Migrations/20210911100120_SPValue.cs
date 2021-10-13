using Microsoft.EntityFrameworkCore.Migrations;

namespace Riode.Domain.Migrations
{
    public partial class SPValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "SpesificationValues",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "SpesificationValues");
        }
    }
}
