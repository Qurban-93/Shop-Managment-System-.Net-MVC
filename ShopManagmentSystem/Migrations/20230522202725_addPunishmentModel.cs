using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class addPunishmentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Moneys_Punishment_ExpensesId",
                table: "Moneys");

            migrationBuilder.DropIndex(
                name: "IX_Moneys_ExpensesId",
                table: "Moneys");

            migrationBuilder.DropColumn(
                name: "ExpensesId",
                table: "Moneys");

            migrationBuilder.AddColumn<int>(
                name: "PunishmentId",
                table: "Salaries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_PunishmentId",
                table: "Salaries",
                column: "PunishmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Salaries_Punishment_PunishmentId",
                table: "Salaries",
                column: "PunishmentId",
                principalTable: "Punishment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salaries_Punishment_PunishmentId",
                table: "Salaries");

            migrationBuilder.DropIndex(
                name: "IX_Salaries_PunishmentId",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "PunishmentId",
                table: "Salaries");

            migrationBuilder.AddColumn<int>(
                name: "ExpensesId",
                table: "Moneys",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Moneys_ExpensesId",
                table: "Moneys",
                column: "ExpensesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Moneys_Punishment_ExpensesId",
                table: "Moneys",
                column: "ExpensesId",
                principalTable: "Punishment",
                principalColumn: "Id");
        }
    }
}
