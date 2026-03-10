using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVerse.Migrations
{
    /// <inheritdoc />
    public partial class UsernameToAdoptionRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AdoptionRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AdoptionRequests");
        }
    }
}
