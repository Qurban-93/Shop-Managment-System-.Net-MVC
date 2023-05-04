using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class changeCustomerEmailInRefundOrderTbAtCustomerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerEmail",
                table: "RefundOrders");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "RefundOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrders_CustomerId",
                table: "RefundOrders",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundOrders_Customers_CustomerId",
                table: "RefundOrders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundOrders_Customers_CustomerId",
                table: "RefundOrders");

            migrationBuilder.DropIndex(
                name: "IX_RefundOrders_CustomerId",
                table: "RefundOrders");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "RefundOrders");

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmail",
                table: "RefundOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
