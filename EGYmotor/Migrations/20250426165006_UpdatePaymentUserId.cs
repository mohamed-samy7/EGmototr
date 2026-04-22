using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EGYmotor.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_RegisterUser_RegisterUserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RegisterUserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RegisterUserId",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_RegisterUser_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "RegisterUser",
                principalColumn: "RegisterUserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_RegisterUser_UserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserId",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "RegisterUserId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RegisterUserId",
                table: "Payments",
                column: "RegisterUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_RegisterUser_RegisterUserId",
                table: "Payments",
                column: "RegisterUserId",
                principalTable: "RegisterUser",
                principalColumn: "RegisterUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
