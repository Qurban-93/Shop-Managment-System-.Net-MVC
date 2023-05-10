using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class addSaleIdForRefundTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "Refunds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_SaleId",
                table: "Refunds",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Sales_SaleId",
                table: "Refunds",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Sales_SaleId",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_SaleId",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "Refunds");
        }
    }
}
