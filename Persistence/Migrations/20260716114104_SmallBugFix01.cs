using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SmallBugFix01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Draft_Users_AuthorId",
                table: "Draft");

            migrationBuilder.DropForeignKey(
                name: "FK_Letter_Users_AddresseeId",
                table: "Letter");

            migrationBuilder.DropForeignKey(
                name: "FK_Letter_Users_RecipientId",
                table: "Letter");

            migrationBuilder.DropForeignKey(
                name: "FK_LetterStates_Letter_LetterId",
                table: "LetterStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Letter",
                table: "Letter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Draft",
                table: "Draft");

            migrationBuilder.RenameTable(
                name: "Letter",
                newName: "Letters");

            migrationBuilder.RenameTable(
                name: "Draft",
                newName: "Drafts");

            migrationBuilder.RenameIndex(
                name: "IX_Letter_RecipientId",
                table: "Letters",
                newName: "IX_Letters_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Letter_AddresseeId",
                table: "Letters",
                newName: "IX_Letters_AddresseeId");

            migrationBuilder.RenameIndex(
                name: "IX_Draft_AuthorId",
                table: "Drafts",
                newName: "IX_Drafts_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Letters",
                table: "Letters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Drafts",
                table: "Drafts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Drafts_Users_AuthorId",
                table: "Drafts",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Users_AddresseeId",
                table: "Letters",
                column: "AddresseeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Users_RecipientId",
                table: "Letters",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LetterStates_Letters_LetterId",
                table: "LetterStates",
                column: "LetterId",
                principalTable: "Letters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drafts_Users_AuthorId",
                table: "Drafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Users_AddresseeId",
                table: "Letters");

            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Users_RecipientId",
                table: "Letters");

            migrationBuilder.DropForeignKey(
                name: "FK_LetterStates_Letters_LetterId",
                table: "LetterStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Letters",
                table: "Letters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Drafts",
                table: "Drafts");

            migrationBuilder.RenameTable(
                name: "Letters",
                newName: "Letter");

            migrationBuilder.RenameTable(
                name: "Drafts",
                newName: "Draft");

            migrationBuilder.RenameIndex(
                name: "IX_Letters_RecipientId",
                table: "Letter",
                newName: "IX_Letter_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Letters_AddresseeId",
                table: "Letter",
                newName: "IX_Letter_AddresseeId");

            migrationBuilder.RenameIndex(
                name: "IX_Drafts_AuthorId",
                table: "Draft",
                newName: "IX_Draft_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Letter",
                table: "Letter",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Draft",
                table: "Draft",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Draft_Users_AuthorId",
                table: "Draft",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_LetterStates_Letter_LetterId",
                table: "LetterStates",
                column: "LetterId",
                principalTable: "Letter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
