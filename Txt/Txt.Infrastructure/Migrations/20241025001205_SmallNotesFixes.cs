using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Txt.Infrastructure.Data;

#nullable disable

namespace Txt.Infrastructure.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241025001205_SmallNotesFixes")]
    public partial class SmallNotesFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "NoteLines",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "NoteLines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "NoteLines",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "NoteLines",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoteLines_CreatedById",
                table: "NoteLines",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NoteLines_ModifiedById",
                table: "NoteLines",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLines_Users_CreatedById",
                table: "NoteLines",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLines_Users_ModifiedById",
                table: "NoteLines",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteLines_Users_CreatedById",
                table: "NoteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteLines_Users_ModifiedById",
                table: "NoteLines");

            migrationBuilder.DropIndex(
                name: "IX_NoteLines_CreatedById",
                table: "NoteLines");

            migrationBuilder.DropIndex(
                name: "IX_NoteLines_ModifiedById",
                table: "NoteLines");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "NoteLines");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "NoteLines");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "NoteLines");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "NoteLines");
        }
    }
}
