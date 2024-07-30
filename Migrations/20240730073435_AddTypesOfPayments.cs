using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarWashManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddTypesOfPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AreCardPaymentsAllowed",
                table: "Stations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AreCashPaymentsAllowed",
                table: "Stations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreCardPaymentsAllowed",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "AreCashPaymentsAllowed",
                table: "Stations");
        }
    }
}
