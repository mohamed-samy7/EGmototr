using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EGYmotor.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFeedbackUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback",
                column: "UserId",
                unique: true);
        }
    }
}
