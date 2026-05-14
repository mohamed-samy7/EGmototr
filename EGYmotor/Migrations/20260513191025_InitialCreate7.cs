using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EGYmotor.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "RegisterUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "RegisterUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
