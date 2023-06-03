using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddMemoryTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemoryId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Memories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemoryCapacity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "MemoryId",
                table: "Products");
        }
    }
}
