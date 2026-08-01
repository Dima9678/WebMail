using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ForwardLetter1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Forwarded",
                table: "Letters",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OriginalAuthorId",
                table: "Letters",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Letters_OriginalAuthorId",
                table: "Letters",
                column: "OriginalAuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Users_OriginalAuthorId",
                table: "Letters",
                column: "OriginalAuthorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Users_OriginalAuthorId",
                table: "Letters");

            migrationBuilder.DropIndex(
                name: "IX_Letters_OriginalAuthorId",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "Forwarded",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "OriginalAuthorId",
                table: "Letters");
        }
    }
}
