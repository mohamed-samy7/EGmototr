using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EGYmotor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFeedbackModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Feedback",
                newName: "FeedbackID");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "RegisterUser",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FeedbackID",
                table: "Feedback",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "RegisterUser",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);
        }
    }
}
