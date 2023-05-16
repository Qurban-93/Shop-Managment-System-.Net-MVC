using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class changeClumnEmloyeeAgeBirthDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Employees");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Employees");

            migrationBuilder.AddColumn<byte>(
                name: "Age",
                table: "Employees",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
