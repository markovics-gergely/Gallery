using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gallery.DAL.Migrations
{
    public partial class JoinTableRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikedAlbums");

            migrationBuilder.CreateTable(
                name: "FavoriteAlbums",
                columns: table => new
                {
                    FavoritedAlbumsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FavoritedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteAlbums", x => new { x.FavoritedAlbumsId, x.FavoritedById });
                    table.ForeignKey(
                        name: "FK_FavoriteAlbums_Albums_FavoritedAlbumsId",
                        column: x => x.FavoritedAlbumsId,
                        principalTable: "Albums",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FavoriteAlbums_AspNetUsers_FavoritedById",
                        column: x => x.FavoritedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteAlbums_FavoritedById",
                table: "FavoriteAlbums",
                column: "FavoritedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteAlbums");

            migrationBuilder.CreateTable(
                name: "LikedAlbums",
                columns: table => new
                {
                    FavoritedAlbumsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FavoritedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedAlbums", x => new { x.FavoritedAlbumsId, x.FavoritedById });
                    table.ForeignKey(
                        name: "FK_LikedAlbums_Albums_FavoritedAlbumsId",
                        column: x => x.FavoritedAlbumsId,
                        principalTable: "Albums",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LikedAlbums_AspNetUsers_FavoritedById",
                        column: x => x.FavoritedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikedAlbums_FavoritedById",
                table: "LikedAlbums",
                column: "FavoritedById");
        }
    }
}
