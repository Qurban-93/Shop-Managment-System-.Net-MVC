using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class deleteRefunIdinProd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Refunds_RefundId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_RefundId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RefundId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RefundId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_RefundId",
                table: "Products",
                column: "RefundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Refunds_RefundId",
                table: "Products",
                column: "RefundId",
                principalTable: "Refunds",
                principalColumn: "Id");
        }
    }
}
