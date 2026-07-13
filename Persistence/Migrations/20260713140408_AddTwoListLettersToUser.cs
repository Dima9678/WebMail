using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoListLettersToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LetterStates_Letter_LetterId",
                table: "LetterStates");

            migrationBuilder.DropForeignKey(
                name: "FK_LetterStates_Users_UserId",
                table: "LetterStates");

            migrationBuilder.AddForeignKey(
                name: "FK_LetterStates_Letter_LetterId",
                table: "LetterStates",
                column: "LetterId",
                principalTable: "Letter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LetterStates_Users_UserId",
                table: "LetterStates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LetterStates_Letter_LetterId",
                table: "LetterStates");

            migrationBuilder.DropForeignKey(
                name: "FK_LetterStates_Users_UserId",
                table: "LetterStates");

            migrationBuilder.AddForeignKey(
                name: "FK_LetterStates_Letter_LetterId",
                table: "LetterStates",
                column: "LetterId",
                principalTable: "Letter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LetterStates_Users_UserId",
                table: "LetterStates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
