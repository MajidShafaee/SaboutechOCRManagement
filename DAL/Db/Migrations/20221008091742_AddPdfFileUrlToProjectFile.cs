using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.DB.Migrations
{
    public partial class AddPdfFileUrlToProjectFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfFileUrl",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfFileUrl",
                table: "ProjectFiles");
        }
    }
}
