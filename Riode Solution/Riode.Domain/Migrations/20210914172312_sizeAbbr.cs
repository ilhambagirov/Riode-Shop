using Microsoft.EntityFrameworkCore.Migrations;

namespace Riode.Domain.Migrations
{
    public partial class sizeAbbr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abbr",
                table: "Size",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbr",
                table: "Size");
        }
    }
}
