using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.Data.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCartFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Customers_CustomerId",
                table: "ShoppingCartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Products_ProductId",
                table: "ShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItems_CustomerId",
                table: "ShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItems_ProductId",
                table: "ShoppingCartItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_CustomerId",
                table: "ShoppingCartItems",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ProductId",
                table: "ShoppingCartItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Customers_CustomerId",
                table: "ShoppingCartItems",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Products_ProductId",
                table: "ShoppingCartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
