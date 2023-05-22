using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class addEmplIdForPunishmentTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Punishment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Punishment_EmployeeId",
                table: "Punishment",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Punishment_Employees_EmployeeId",
                table: "Punishment",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Punishment_Employees_EmployeeId",
                table: "Punishment");

            migrationBuilder.DropIndex(
                name: "IX_Punishment_EmployeeId",
                table: "Punishment");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Punishment");
        }
    }
}
