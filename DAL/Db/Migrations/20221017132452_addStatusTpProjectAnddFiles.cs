using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.DB.Migrations
{
    public partial class addStatusTpProjectAnddFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReadAllFiles",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProjectFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadAllFiles",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProjectFiles");
        }
    }
}
