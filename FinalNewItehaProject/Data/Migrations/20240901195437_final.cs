using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalNewItehaProject.Data.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductsModel",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfProducts = table.Column<int>(type: "int", nullable: false),
                    ProductImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsModel", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "UserModel",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModel", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartModel",
                columns: table => new
                {
                    ShoppingCartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartModel", x => x.ShoppingCartId);
                    table.ForeignKey(
                        name: "FK_ShoppingCartModel_UserModel_UserId",
                        column: x => x.UserId,
                        principalTable: "UserModel",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartProductModel",
                columns: table => new
                {
                    ShoppingCartProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingCartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartProductModel", x => x.ShoppingCartProductId);
                    table.ForeignKey(
                        name: "FK_ShoppingCartProductModel_ProductsModel_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductsModel",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartProductModel_ShoppingCartModel_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCartModel",
                        principalColumn: "ShoppingCartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartModel_UserId",
                table: "ShoppingCartModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartProductModel_ProductId",
                table: "ShoppingCartProductModel",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartProductModel_ShoppingCartId",
                table: "ShoppingCartProductModel",
                column: "ShoppingCartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCartProductModel");

            migrationBuilder.DropTable(
                name: "ProductsModel");

            migrationBuilder.DropTable(
                name: "ShoppingCartModel");

            migrationBuilder.DropTable(
                name: "UserModel");
        }
    }
}
