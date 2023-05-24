using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class DisplacementTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplacementId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Displacement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    SenderBranch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationId = table.Column<int>(type: "int", nullable: false),
                    DestinationBranch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Displacement", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_DisplacementId",
                table: "Products",
                column: "DisplacementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Displacement_DisplacementId",
                table: "Products",
                column: "DisplacementId",
                principalTable: "Displacement",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Displacement_DisplacementId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Displacement");

            migrationBuilder.DropIndex(
                name: "IX_Products_DisplacementId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DisplacementId",
                table: "Products");
        }
    }
}
