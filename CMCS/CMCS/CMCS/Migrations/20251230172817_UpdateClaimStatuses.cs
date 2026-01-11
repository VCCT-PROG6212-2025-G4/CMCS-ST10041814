using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMCS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClaimStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PamentStatus",
                table: "Claims",
                newName: "PaymentStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Claims",
                newName: "PamentStatus");
        }
    }
}
