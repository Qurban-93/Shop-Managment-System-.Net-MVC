using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class addExpensesAndExpensesCategoryAndMoneyTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductImages");

            migrationBuilder.CreateTable(
                name: "ExpensesCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensesCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expensess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Descpription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpensesCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expensess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expensess_ExpensesCategories_ExpensesCategoryId",
                        column: x => x.ExpensesCategoryId,
                        principalTable: "ExpensesCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Moneys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Incoming = table.Column<double>(type: "float", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: true),
                    RefundId = table.Column<int>(type: "int", nullable: true),
                    ExpensesId = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moneys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Moneys_Expensess_ExpensesId",
                        column: x => x.ExpensesId,
                        principalTable: "Expensess",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Moneys_Refunds_RefundId",
                        column: x => x.RefundId,
                        principalTable: "Refunds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Moneys_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expensess_ExpensesCategoryId",
                table: "Expensess",
                column: "ExpensesCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Moneys_ExpensesId",
                table: "Moneys",
                column: "ExpensesId");

            migrationBuilder.CreateIndex(
                name: "IX_Moneys_RefundId",
                table: "Moneys",
                column: "RefundId");

            migrationBuilder.CreateIndex(
                name: "IX_Moneys_SaleId",
                table: "Moneys",
                column: "SaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moneys");

            migrationBuilder.DropTable(
                name: "Expensess");

            migrationBuilder.DropTable(
                name: "ExpensesCategories");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
