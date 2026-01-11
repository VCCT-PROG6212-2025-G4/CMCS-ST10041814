using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMCS.Migrations
{
    /// <inheritdoc />
    public partial class addHR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ContactNumber", "Email", "Password", "Role", "UserCode", "Username" },
                values: new object[] { 1, "1234567890", "hr@gmail.com", "0000", "HR", "HR", "HR" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
