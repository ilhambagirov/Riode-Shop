using Microsoft.EntityFrameworkCore.Migrations;

namespace Riode.Domain.Migrations
{
    public partial class sptiny : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpesificationCategoryCollection_Spesifications_SpesificationId",
                table: "SpesificationCategoryCollection");

            migrationBuilder.DropColumn(
                name: "SpesificatiionId",
                table: "SpesificationCategoryCollection");

            migrationBuilder.AlterColumn<int>(
                name: "SpesificationId",
                table: "SpesificationCategoryCollection",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SpesificationCategoryCollection_Spesifications_SpesificationId",
                table: "SpesificationCategoryCollection",
                column: "SpesificationId",
                principalTable: "Spesifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpesificationCategoryCollection_Spesifications_SpesificationId",
                table: "SpesificationCategoryCollection");

            migrationBuilder.AlterColumn<int>(
                name: "SpesificationId",
                table: "SpesificationCategoryCollection",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SpesificatiionId",
                table: "SpesificationCategoryCollection",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_SpesificationCategoryCollection_Spesifications_SpesificationId",
                table: "SpesificationCategoryCollection",
                column: "SpesificationId",
                principalTable: "Spesifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
