using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letter_Users_AddresseeId",
                table: "Letter");

            migrationBuilder.DropForeignKey(
                name: "FK_Letter_Users_RecipientId",
                table: "Letter");

            migrationBuilder.AddForeignKey(
                name: "FK_Letter_Users_AddresseeId",
                table: "Letter",
                column: "AddresseeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Letter_Users_RecipientId",
                table: "Letter",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letter_Users_AddresseeId",
                table: "Letter");

            migrationBuilder.DropForeignKey(
                name: "FK_Letter_Users_RecipientId",
                table: "Letter");

            migrationBuilder.AddForeignKey(
                name: "FK_Letter_Users_AddresseeId",
                table: "Letter",
                column: "AddresseeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Letter_Users_RecipientId",
                table: "Letter",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
