using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class addDescrForRefundOrderandRefund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Refunds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RefundOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RefundOrders");
        }
    }
}
