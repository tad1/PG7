using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removecompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employments_Companies_Companyid",
                table: "Employments");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Employments_Companyid",
                table: "Employments");

            migrationBuilder.DropColumn(
                name: "Companyid",
                table: "Employments");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Employments",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Employments");

            migrationBuilder.AddColumn<Guid>(
                name: "Companyid",
                table: "Employments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employments_Companyid",
                table: "Employments",
                column: "Companyid");

            migrationBuilder.AddForeignKey(
                name: "FK_Employments_Companies_Companyid",
                table: "Employments",
                column: "Companyid",
                principalTable: "Companies",
                principalColumn: "id");
        }
    }
}
