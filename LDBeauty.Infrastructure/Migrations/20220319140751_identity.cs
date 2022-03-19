using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LDBeauty.Infrastructure.Migrations
{
    public partial class identity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientsImages_Clients_ClientId",
                table: "ClientsImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientsProducts_Clients_ClientId",
                table: "ClientsProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ClientImage");

            migrationBuilder.DropTable(
                name: "ClientProduct");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ClientId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientsProducts",
                table: "ClientsProducts");

            migrationBuilder.DropIndex(
                name: "IX_ClientsProducts_ClientId",
                table: "ClientsProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientsImages",
                table: "ClientsImages");

            migrationBuilder.DropIndex(
                name: "IX_ClientsImages_ClientId",
                table: "ClientsImages");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ClientsProducts");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ClientsImages");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ClientsProducts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ClientsImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "nvarchar(17)",
                maxLength: 17,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientsProducts",
                table: "ClientsProducts",
                columns: new[] { "ProductId", "ApplicationUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientsImages",
                table: "ClientsImages",
                columns: new[] { "ImageId", "ApplicationUserId" });

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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUserImage_Images_FavouriteImagesId",
                        column: x => x.FavouriteImagesId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUserProduct_Products_FavouriteProductsId",
                        column: x => x.FavouriteProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientsProducts_ApplicationUserId",
                table: "ClientsProducts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientsImages_ApplicationUserId",
                table: "ClientsImages",
                column: "ApplicationUserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsImages_AspNetUsers_ApplicationUserId",
                table: "ClientsImages",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsProducts_AspNetUsers_ApplicationUserId",
                table: "ClientsProducts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientsImages_AspNetUsers_ApplicationUserId",
                table: "ClientsImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientsProducts_AspNetUsers_ApplicationUserId",
                table: "ClientsProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationUserId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ApplicationUserImage");

            migrationBuilder.DropTable(
                name: "ApplicationUserProduct");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientsProducts",
                table: "ClientsProducts");

            migrationBuilder.DropIndex(
                name: "IX_ClientsProducts_ApplicationUserId",
                table: "ClientsProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientsImages",
                table: "ClientsImages");

            migrationBuilder.DropIndex(
                name: "IX_ClientsImages_ApplicationUserId",
                table: "ClientsImages");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ClientsProducts");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ClientsImages");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "ClientsProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "ClientsImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientsProducts",
                table: "ClientsProducts",
                columns: new[] { "ProductId", "ClientId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientsImages",
                table: "ClientsImages",
                columns: new[] { "ImageId", "ClientId" });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientImage",
                columns: table => new
                {
                    FavouriteImagesId = table.Column<int>(type: "int", nullable: false),
                    UsersFaovuriteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientImage", x => new { x.FavouriteImagesId, x.UsersFaovuriteId });
                    table.ForeignKey(
                        name: "FK_ClientImage_Clients_UsersFaovuriteId",
                        column: x => x.UsersFaovuriteId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientImage_Images_FavouriteImagesId",
                        column: x => x.FavouriteImagesId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientProduct",
                columns: table => new
                {
                    FavouriteProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersFavouriteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProduct", x => new { x.FavouriteProductsId, x.UsersFavouriteId });
                    table.ForeignKey(
                        name: "FK_ClientProduct_Clients_UsersFavouriteId",
                        column: x => x.UsersFavouriteId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientProduct_Products_FavouriteProductsId",
                        column: x => x.FavouriteProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientsProducts_ClientId",
                table: "ClientsProducts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientsImages_ClientId",
                table: "ClientsImages",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientImage_UsersFaovuriteId",
                table: "ClientImage",
                column: "UsersFaovuriteId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProduct_UsersFavouriteId",
                table: "ClientProduct",
                column: "UsersFavouriteId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email",
                table: "Clients",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsImages_Clients_ClientId",
                table: "ClientsImages",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsProducts_Clients_ClientId",
                table: "ClientsProducts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
