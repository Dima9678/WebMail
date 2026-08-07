using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGender2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LetterStates_Letters_LetterId",
                table: "LetterStates");

            migrationBuilder.AddForeignKey(
                name: "FK_LetterStates_Letters_LetterId",
                table: "LetterStates",
                column: "LetterId",
                principalTable: "Letters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LetterStates_Letters_LetterId",
                table: "LetterStates");

            migrationBuilder.AddForeignKey(
                name: "FK_LetterStates_Letters_LetterId",
                table: "LetterStates",
                column: "LetterId",
                principalTable: "Letters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
