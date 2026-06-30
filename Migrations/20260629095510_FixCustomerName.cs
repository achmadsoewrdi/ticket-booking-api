using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TikectingBooking.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixCustomerName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomeName",
                table: "Tickets",
                newName: "CustomerName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "Tickets",
                newName: "CustomeName");
        }
    }
}
