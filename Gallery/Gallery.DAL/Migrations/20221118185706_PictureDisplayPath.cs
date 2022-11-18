using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gallery.DAL.Migrations
{
    public partial class PictureDisplayPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Pictures",
                newName: "PhysicalPath");

            migrationBuilder.AddColumn<string>(
                name: "DisplayPath",
                table: "Pictures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayPath",
                table: "Pictures");

            migrationBuilder.RenameColumn(
                name: "PhysicalPath",
                table: "Pictures",
                newName: "Path");
        }
    }
}
