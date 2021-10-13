using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riode.Domain.Migrations
{
    public partial class Specification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spesifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteByUserId = table.Column<int>(type: "int", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spesifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpesificationCategoryCollection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpesificatiionId = table.Column<int>(type: "int", nullable: false),
                    SpesificationId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteByUserId = table.Column<int>(type: "int", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpesificationCategoryCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpesificationCategoryCollection_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpesificationCategoryCollection_Spesifications_SpesificationId",
                        column: x => x.SpesificationId,
                        principalTable: "Spesifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpesificationValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpesificationId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteByUserId = table.Column<int>(type: "int", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpesificationValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpesificationValues_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpesificationValues_Spesifications_SpesificationId",
                        column: x => x.SpesificationId,
                        principalTable: "Spesifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpesificationCategoryCollection_CategoryId",
                table: "SpesificationCategoryCollection",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SpesificationCategoryCollection_SpesificationId",
                table: "SpesificationCategoryCollection",
                column: "SpesificationId");

            migrationBuilder.CreateIndex(
                name: "IX_SpesificationValues_ProductId",
                table: "SpesificationValues",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SpesificationValues_SpesificationId",
                table: "SpesificationValues",
                column: "SpesificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpesificationCategoryCollection");

            migrationBuilder.DropTable(
                name: "SpesificationValues");

            migrationBuilder.DropTable(
                name: "Spesifications");
        }
    }
}
