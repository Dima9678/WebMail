using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ManyRecipients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Users_OriginalAuthorId",
                table: "Letters");

            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Users_RecipientId",
                table: "Letters");

            migrationBuilder.DropIndex(
                name: "IX_Letters_RecipientId",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Letters");

            migrationBuilder.RenameColumn(
                name: "RecipientEmail",
                table: "Drafts",
                newName: "Recipients");

            migrationBuilder.AlterColumn<bool>(
                name: "Forwarded",
                table: "Letters",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ForwardRecipientId",
                table: "Letters",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LetterUser",
                columns: table => new
                {
                    AcceptLettersId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterUser", x => new { x.AcceptLettersId, x.RecipientsId });
                    table.ForeignKey(
                        name: "FK_LetterUser_Letters_AcceptLettersId",
                        column: x => x.AcceptLettersId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LetterUser_Users_RecipientsId",
                        column: x => x.RecipientsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Letters_ForwardRecipientId",
                table: "Letters",
                column: "ForwardRecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterUser_RecipientsId",
                table: "LetterUser",
                column: "RecipientsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Letters_ParentLetterId",
                table: "Letters",
                column: "ParentLetterId",
                principalTable: "Letters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Users_ForwardRecipientId",
                table: "Letters",
                column: "ForwardRecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Users_OriginalAuthorId",
                table: "Letters",
                column: "OriginalAuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Letters_ParentLetterId",
                table: "Letters");

            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Users_ForwardRecipientId",
                table: "Letters");

            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Users_OriginalAuthorId",
                table: "Letters");

            migrationBuilder.DropTable(
                name: "LetterUser");

            migrationBuilder.DropIndex(
                name: "IX_Letters_ForwardRecipientId",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "ForwardRecipientId",
                table: "Letters");

            migrationBuilder.RenameColumn(
                name: "Recipients",
                table: "Drafts",
                newName: "RecipientEmail");

            migrationBuilder.AlterColumn<bool>(
                name: "Forwarded",
                table: "Letters",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RecipientId",
                table: "Letters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Letters_RecipientId",
                table: "Letters",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Letters_ParentLetterId",
                table: "Letters",
                column: "ParentLetterId",
                principalTable: "Letters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Users_OriginalAuthorId",
                table: "Letters",
                column: "OriginalAuthorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Users_RecipientId",
                table: "Letters",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
