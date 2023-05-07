using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class deleteTBREfundProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefundProducts");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Refunds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_ProductId",
                table: "Refunds",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Products_ProductId",
                table: "Refunds",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Products_ProductId",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_ProductId",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Refunds");

            migrationBuilder.CreateTable(
                name: "RefundProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RefundId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefundProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RefundProducts_Refunds_RefundId",
                        column: x => x.RefundId,
                        principalTable: "Refunds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefundProducts_ProductId",
                table: "RefundProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundProducts_RefundId",
                table: "RefundProducts",
                column: "RefundId");
        }
    }
}
