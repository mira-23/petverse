using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetVerse.Migrations
{
    /// <inheritdoc />
    public partial class EngagementPerUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Engagements_EventPostId",
                table: "Engagements");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Engagements",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Engagements_EventPostId_UserId_Type",
                table: "Engagements",
                columns: new[] { "EventPostId", "UserId", "Type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Engagements_EventPostId_UserId_Type",
                table: "Engagements");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Engagements",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Engagements_EventPostId",
                table: "Engagements",
                column: "EventPostId");
        }
    }
}
