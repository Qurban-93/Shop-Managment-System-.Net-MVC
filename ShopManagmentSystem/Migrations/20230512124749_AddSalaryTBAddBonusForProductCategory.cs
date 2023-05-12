using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddSalaryTBAddBonusForProductCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeePostions_EmployeePostionId",
                table: "Employees");

            migrationBuilder.AddColumn<double>(
                name: "Bonus",
                table: "ProductCategories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeePostionId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Salaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bonus = table.Column<double>(type: "float", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: true),
                    RefundId = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salaries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salaries_Refunds_RefundId",
                        column: x => x.RefundId,
                        principalTable: "Refunds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Salaries_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_EmployeeId",
                table: "Salaries",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_RefundId",
                table: "Salaries",
                column: "RefundId");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_SaleId",
                table: "Salaries",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeePostions_EmployeePostionId",
                table: "Employees",
                column: "EmployeePostionId",
                principalTable: "EmployeePostions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeePostions_EmployeePostionId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Salaries");

            migrationBuilder.DropColumn(
                name: "Bonus",
                table: "ProductCategories");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeePostionId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeePostions_EmployeePostionId",
                table: "Employees",
                column: "EmployeePostionId",
                principalTable: "EmployeePostions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
