using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVerse.Migrations
{
    /// <inheritdoc />
    public partial class Comments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LostAnimalPostId = table.Column<int>(type: "int", nullable: true),
                    AnimalAdoptionPostId = table.Column<int>(type: "int", nullable: true),
                    BusinessPostId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AnimalAdoptionPosts_AnimalAdoptionPostId",
                        column: x => x.AnimalAdoptionPostId,
                        principalTable: "AnimalAdoptionPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_BusinessPosts_BusinessPostId",
                        column: x => x.BusinessPostId,
                        principalTable: "BusinessPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_LostAnimalPosts_LostAnimalPostId",
                        column: x => x.LostAnimalPostId,
                        principalTable: "LostAnimalPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AnimalAdoptionPostId",
                table: "Comments",
                column: "AnimalAdoptionPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BusinessPostId",
                table: "Comments",
                column: "BusinessPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_LostAnimalPostId",
                table: "Comments",
                column: "LostAnimalPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");
        }
    }
}
