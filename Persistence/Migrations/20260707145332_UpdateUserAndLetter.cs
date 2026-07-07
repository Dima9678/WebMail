using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAndLetter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSent",
                table: "Letters",
                newName: "Starred");

            migrationBuilder.AddColumn<bool>(
                name: "IsReaden",
                table: "Letters",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReaden",
                table: "Letters");

            migrationBuilder.RenameColumn(
                name: "Starred",
                table: "Letters",
                newName: "IsSent");
        }
    }
}
