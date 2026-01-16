using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVerse.Migrations
{
    /// <inheritdoc />
    public partial class OptionalPet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Pet_PetId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "PetId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Pet_PetId",
                table: "AspNetUsers",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Pet_PetId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "PetId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Pet_PetId",
                table: "AspNetUsers",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
