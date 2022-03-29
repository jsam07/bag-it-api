using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bagit_api.Data.Migrations
{
    public partial class M1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    ListId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: true),
                    IsEditable = table.Column<bool>(type: "INTEGER", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.ListId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 70, nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: true),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListProduct",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    ListId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListProduct", x => new { x.ListId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ShoppingListProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingListProduct_ShoppingLists_ListId",
                        column: x => x.ListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "ListId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserShoppingList",
                columns: table => new
                {
                    ListId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserShoppingList", x => new { x.ListId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserShoppingList_ShoppingLists_ListId",
                        column: x => x.ListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "ListId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserShoppingList_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ShoppingLists",
                columns: new[] { "ListId", "Description", "IsEditable", "IsPublic", "Name", "Notes" },
                values: new object[] { 1, null, null, null, "Test Shopping List", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "CreatedAt", "Email", "FirstName", "LastName", "LoginProvider", "ModifiedAt", "Password", "Phone", "ProviderDisplayName", "ProviderKey", "Username" },
                values: new object[] { 1, null, null, "test@gmail.com", null, null, null, null, null, null, null, null, "test_user" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListProduct_ProductId",
                table: "ShoppingListProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserShoppingList_UserId",
                table: "UserShoppingList",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingListProduct");

            migrationBuilder.DropTable(
                name: "UserShoppingList");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ShoppingLists");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
