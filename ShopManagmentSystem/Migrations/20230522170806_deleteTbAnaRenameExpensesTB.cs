using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class deleteTbAnaRenameExpensesTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Moneys_Expensess_ExpensesId",
                table: "Moneys");

            migrationBuilder.DropTable(
                name: "Expensess");

            migrationBuilder.DropTable(
                name: "ExpensesCategories");

            migrationBuilder.CreateTable(
                name: "Punishment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Descpription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Punishment", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Moneys_Punishment_ExpensesId",
                table: "Moneys",
                column: "ExpensesId",
                principalTable: "Punishment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Moneys_Punishment_ExpensesId",
                table: "Moneys");

            migrationBuilder.DropTable(
                name: "Punishment");

            migrationBuilder.CreateTable(
                name: "ExpensesCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descpription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpensesCategoryId = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Expensess_ExpensesCategoryId",
                table: "Expensess",
                column: "ExpensesCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Moneys_Expensess_ExpensesId",
                table: "Moneys",
                column: "ExpensesId",
                principalTable: "Expensess",
                principalColumn: "Id");
        }
    }
}
