using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMCS.Migrations
{
    /// <inheritdoc />
    public partial class ClaimModelAddEnumPaymentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Claims");

            migrationBuilder.AddColumn<int>(
                name: "PamentStatus",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PamentStatus",
                table: "Claims");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Claims",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
