using Microsoft.EntityFrameworkCore.Migrations;

namespace Riode.Domain.Migrations
{
    public partial class Mark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Marked",
                table: "ContactPosts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Marked",
                table: "ContactPosts");
        }
    }
}
