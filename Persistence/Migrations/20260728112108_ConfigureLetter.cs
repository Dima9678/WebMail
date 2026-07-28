using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureLetter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Letters_ParentLetterId",
                table: "Letters");

            migrationBuilder.DropIndex(
                name: "IX_Letters_ParentLetterId",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "ChildLetterId",
                table: "Letters");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentLetterId",
                table: "Letters",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<List<Guid>>(
                name: "ChildrenLettersId",
                table: "Letters",
                type: "uuid[]",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentLetterId",
                table: "Letters",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Letters_ParentLetterId",
                table: "Letters",
                column: "ParentLetterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Letters_ParentLetterId",
                table: "Letters",
                column: "ParentLetterId",
                principalTable: "Letters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Letters_ParentLetterId",
                table: "Letters");

            migrationBuilder.DropIndex(
                name: "IX_Letters_ParentLetterId",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "ChildrenLettersId",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "ParentLetterId",
                table: "Letters");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentLetterId",
                table: "Letters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChildLetterId",
                table: "Letters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Letters_ParentLetterId",
                table: "Letters",
                column: "ParentLetterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Letters_ParentLetterId",
                table: "Letters",
                column: "ParentLetterId",
                principalTable: "Letters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
