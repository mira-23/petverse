using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVerse.Migrations
{
    /// <inheritdoc />
    public partial class PetAdditionToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PetId",
                table: "AspNetUsers",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Pet_PetId",
                table: "AspNetUsers",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Pet_PetId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PetId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "AspNetUsers");
        }
    }
}
