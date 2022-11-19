using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gallery.DAL.Migrations
{
    public partial class LikeCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedAlbums_Albums_LikedAlbumsId",
                table: "LikedAlbums");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedAlbums_AspNetUsers_LikedById",
                table: "LikedAlbums");

            migrationBuilder.RenameColumn(
                name: "LikedById",
                table: "LikedAlbums",
                newName: "FavoritedById");

            migrationBuilder.RenameColumn(
                name: "LikedAlbumsId",
                table: "LikedAlbums",
                newName: "FavoritedAlbumsId");

            migrationBuilder.RenameIndex(
                name: "IX_LikedAlbums_LikedById",
                table: "LikedAlbums",
                newName: "IX_LikedAlbums_FavoritedById");

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedAlbums_Albums_FavoritedAlbumsId",
                table: "LikedAlbums",
                column: "FavoritedAlbumsId",
                principalTable: "Albums",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedAlbums_AspNetUsers_FavoritedById",
                table: "LikedAlbums",
                column: "FavoritedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedAlbums_Albums_FavoritedAlbumsId",
                table: "LikedAlbums");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedAlbums_AspNetUsers_FavoritedById",
                table: "LikedAlbums");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "FavoritedById",
                table: "LikedAlbums",
                newName: "LikedById");

            migrationBuilder.RenameColumn(
                name: "FavoritedAlbumsId",
                table: "LikedAlbums",
                newName: "LikedAlbumsId");

            migrationBuilder.RenameIndex(
                name: "IX_LikedAlbums_FavoritedById",
                table: "LikedAlbums",
                newName: "IX_LikedAlbums_LikedById");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedAlbums_Albums_LikedAlbumsId",
                table: "LikedAlbums",
                column: "LikedAlbumsId",
                principalTable: "Albums",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedAlbums_AspNetUsers_LikedById",
                table: "LikedAlbums",
                column: "LikedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
