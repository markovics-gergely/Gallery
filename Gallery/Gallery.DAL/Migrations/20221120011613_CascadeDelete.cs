using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gallery.DAL.Migrations
{
    public partial class CascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteAlbums_Albums_FavoritedAlbumsId",
                table: "FavoriteAlbums");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteAlbums_Albums_FavoritedAlbumsId",
                table: "FavoriteAlbums",
                column: "FavoritedAlbumsId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteAlbums_Albums_FavoritedAlbumsId",
                table: "FavoriteAlbums");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteAlbums_Albums_FavoritedAlbumsId",
                table: "FavoriteAlbums",
                column: "FavoritedAlbumsId",
                principalTable: "Albums",
                principalColumn: "Id");
        }
    }
}
