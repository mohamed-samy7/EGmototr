using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EGYmotor.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Requests");
        }
    }
}
