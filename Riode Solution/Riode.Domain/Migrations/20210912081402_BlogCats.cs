using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riode.Domain.Migrations
{
    public partial class BlogCats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "BlogCategoryId",
            //    table: "Blogs",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "BlogCategories",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ParentId = table.Column<int>(type: "int", nullable: true),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedByUserId = table.Column<int>(type: "int", nullable: true),
            //        CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        DeleteByUserId = table.Column<int>(type: "int", nullable: true),
            //        DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BlogCategories", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_BlogCategories_BlogCategories_ParentId",
            //            column: x => x.ParentId,
            //            principalTable: "BlogCategories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Blogs_BlogCategoryId",
            //    table: "Blogs",
            //    column: "BlogCategoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BlogCategories_ParentId",
            //    table: "BlogCategories",
            //    column: "ParentId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Blogs_BlogCategories_BlogCategoryId",
            //    table: "Blogs",
            //    column: "BlogCategoryId",
            //    principalTable: "BlogCategories",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Blogs_BlogCategories_BlogCategoryId",
            //    table: "Blogs");

            //migrationBuilder.DropTable(
            //    name: "BlogCategories");

            //migrationBuilder.DropIndex(
            //    name: "IX_Blogs_BlogCategoryId",
            //    table: "Blogs");

            //migrationBuilder.DropColumn(
            //    name: "BlogCategoryId",
            //    table: "Blogs");
        }
    }
}
