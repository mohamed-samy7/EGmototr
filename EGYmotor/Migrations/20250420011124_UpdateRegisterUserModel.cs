using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EGYmotor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRegisterUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_RegisterUser_RegisterUserUserId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RegisterUser",
                newName: "RegisterUserId");

            migrationBuilder.RenameColumn(
                name: "RegisterUserUserId",
                table: "Payments",
                newName: "RegisterUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_RegisterUserUserId",
                table: "Payments",
                newName: "IX_Payments_RegisterUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_RegisterUser_RegisterUserId",
                table: "Payments",
                column: "RegisterUserId",
                principalTable: "RegisterUser",
                principalColumn: "RegisterUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_RegisterUser_RegisterUserId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "RegisterUserId",
                table: "RegisterUser",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "RegisterUserId",
                table: "Payments",
                newName: "RegisterUserUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_RegisterUserId",
                table: "Payments",
                newName: "IX_Payments_RegisterUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_RegisterUser_RegisterUserUserId",
                table: "Payments",
                column: "RegisterUserUserId",
                principalTable: "RegisterUser",
                principalColumn: "UserId");
        }
    }
}
