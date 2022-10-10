using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.DB.Migrations
{
    public partial class AddExtraFieldsToProjectFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorsEn",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorsFa",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISSN",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Issue",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JournalName",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PdfFilePath",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleFa",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Volume",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "ProjectFiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorsEn",
                table: "ProjectFiles");

            migrationBuilder.DropColumn(
                name: "AuthorsFa",
                table: "ProjectFiles");

            migrationBuilder.DropColumn(
                name: "ISSN",
                table: "ProjectFiles");

            migrationBuilder.DropColumn(
                name: "Issue",
                table: "ProjectFiles");

            migrationBuilder.DropColumn(
                name: "JournalName",
                table: "ProjectFiles");

            migrationBuilder.DropColumn(
                name: "PdfFilePath",
                table: "ProjectFiles");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "ProjectFiles");

            migrationBuilder.DropColumn(
                name: "TitleFa",
                table: "ProjectFiles");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "ProjectFiles");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "ProjectFiles");
        }
    }
}
