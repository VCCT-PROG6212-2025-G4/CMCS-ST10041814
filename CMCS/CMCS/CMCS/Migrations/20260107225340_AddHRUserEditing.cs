using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMCS.Migrations
{
    /// <inheritdoc />
    public partial class AddHRUserEditing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Users_UserID",
                table: "Claims");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Users_UserID",
                table: "Claims",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Users_UserID",
                table: "Claims");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Users_UserID",
                table: "Claims",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
