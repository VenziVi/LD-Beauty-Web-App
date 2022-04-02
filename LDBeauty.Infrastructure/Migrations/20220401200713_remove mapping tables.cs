using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LDBeauty.Infrastructure.Migrations
{
    public partial class removemappingtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserImage");

            migrationBuilder.DropTable(
                name: "ApplicationUserProduct");

            migrationBuilder.DropTable(
                name: "UsersImages");

            migrationBuilder.DropTable(
                name: "UsersProducts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Images",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ApplicationUserId",
                table: "Products",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ApplicationUserId",
                table: "Images",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_AspNetUsers_ApplicationUserId",
                table: "Images",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_ApplicationUserId",
                table: "Products",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_AspNetUsers_ApplicationUserId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_ApplicationUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ApplicationUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Images_ApplicationUserId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Images");

            migrationBuilder.CreateTable(
                name: "ApplicationUserImage",
                columns: table => new
                {
                    FavouriteImagesId = table.Column<int>(type: "int", nullable: false),
                    UsersFaovuriteId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserImage", x => new { x.FavouriteImagesId, x.UsersFaovuriteId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserImage_AspNetUsers_UsersFaovuriteId",
                        column: x => x.UsersFaovuriteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserImage_Images_FavouriteImagesId",
                        column: x => x.FavouriteImagesId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserProduct",
                columns: table => new
                {
                    FavouriteProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersFavouriteId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserProduct", x => new { x.FavouriteProductsId, x.UsersFavouriteId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserProduct_AspNetUsers_UsersFavouriteId",
                        column: x => x.UsersFavouriteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserProduct_Products_FavouriteProductsId",
                        column: x => x.FavouriteProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersImages",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersImages", x => new { x.ImageId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_UsersImages_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersImages_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersProducts",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersProducts", x => new { x.ProductId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_UsersProducts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserImage_UsersFaovuriteId",
                table: "ApplicationUserImage",
                column: "UsersFaovuriteId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserProduct_UsersFavouriteId",
                table: "ApplicationUserProduct",
                column: "UsersFavouriteId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersImages_ApplicationUserId",
                table: "UsersImages",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersProducts_ApplicationUserId",
                table: "UsersProducts",
                column: "ApplicationUserId");
        }
    }
}
