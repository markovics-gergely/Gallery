using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gallery.DAL.Migrations
{
    public partial class TableNamingFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_AspNetUsers_CreatorId",
                table: "Album");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedAlbums_Album_LikedAlbumsId",
                table: "LikedAlbums");

            migrationBuilder.DropForeignKey(
                name: "FK_Picture_Album_AlbumId",
                table: "Picture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Picture",
                table: "Picture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Album",
                table: "Album");

            migrationBuilder.RenameTable(
                name: "Picture",
                newName: "Pictures");

            migrationBuilder.RenameTable(
                name: "Album",
                newName: "Albums");

            migrationBuilder.RenameIndex(
                name: "IX_Picture_AlbumId",
                table: "Pictures",
                newName: "IX_Pictures_AlbumId");

            migrationBuilder.RenameIndex(
                name: "IX_Album_CreatorId",
                table: "Albums",
                newName: "IX_Albums_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Albums",
                table: "Albums",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_AspNetUsers_CreatorId",
                table: "Albums",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedAlbums_Albums_LikedAlbumsId",
                table: "LikedAlbums",
                column: "LikedAlbumsId",
                principalTable: "Albums",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Albums_AlbumId",
                table: "Pictures",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_AspNetUsers_CreatorId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedAlbums_Albums_LikedAlbumsId",
                table: "LikedAlbums");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Albums_AlbumId",
                table: "Pictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Albums",
                table: "Albums");

            migrationBuilder.RenameTable(
                name: "Pictures",
                newName: "Picture");

            migrationBuilder.RenameTable(
                name: "Albums",
                newName: "Album");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_AlbumId",
                table: "Picture",
                newName: "IX_Picture_AlbumId");

            migrationBuilder.RenameIndex(
                name: "IX_Albums_CreatorId",
                table: "Album",
                newName: "IX_Album_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Picture",
                table: "Picture",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Album",
                table: "Album",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Album_AspNetUsers_CreatorId",
                table: "Album",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedAlbums_Album_LikedAlbumsId",
                table: "LikedAlbums",
                column: "LikedAlbumsId",
                principalTable: "Album",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_Album_AlbumId",
                table: "Picture",
                column: "AlbumId",
                principalTable: "Album",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
