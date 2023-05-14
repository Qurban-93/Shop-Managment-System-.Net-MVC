using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class addRelationBetweenProdCatProdModelBrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "ProductModels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryId",
                table: "ProductModels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductModels_BrandId",
                table: "ProductModels",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductModels_ProductCategoryId",
                table: "ProductModels",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductModels_Brands_BrandId",
                table: "ProductModels",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductModels_ProductCategories_ProductCategoryId",
                table: "ProductModels",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductModels_Brands_BrandId",
                table: "ProductModels");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductModels_ProductCategories_ProductCategoryId",
                table: "ProductModels");

            migrationBuilder.DropIndex(
                name: "IX_ProductModels_BrandId",
                table: "ProductModels");

            migrationBuilder.DropIndex(
                name: "IX_ProductModels_ProductCategoryId",
                table: "ProductModels");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "ProductModels");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId",
                table: "ProductModels");
        }
    }
}
