using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class deleteMemoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Memories_MemoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Memories");

            migrationBuilder.DropIndex(
                name: "IX_Products_MemoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Memory",
                table: "RefundOrders");

            migrationBuilder.DropColumn(
                name: "MemoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Memory",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Memory",
                table: "RefundOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemoryId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Memory",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Memories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemoryCapacity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_MemoryId",
                table: "Products",
                column: "MemoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Memories_MemoryId",
                table: "Products",
                column: "MemoryId",
                principalTable: "Memories",
                principalColumn: "Id");
        }
    }
}
