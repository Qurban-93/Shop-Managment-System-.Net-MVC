using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class addBranchIdMoneyTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Moneys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Moneys_BranchId",
                table: "Moneys",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Moneys_Branches_BranchId",
                table: "Moneys",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Moneys_Branches_BranchId",
                table: "Moneys");

            migrationBuilder.DropIndex(
                name: "IX_Moneys_BranchId",
                table: "Moneys");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Moneys");
        }
    }
}
