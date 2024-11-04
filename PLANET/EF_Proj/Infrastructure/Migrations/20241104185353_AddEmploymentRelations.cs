using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmploymentRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PersonId1",
                table: "Employments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employments_PersonId1",
                table: "Employments",
                column: "PersonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Employments_People_PersonId1",
                table: "Employments",
                column: "PersonId1",
                principalTable: "People",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employments_People_PersonId1",
                table: "Employments");

            migrationBuilder.DropIndex(
                name: "IX_Employments_PersonId1",
                table: "Employments");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "Employments");
        }
    }
}
