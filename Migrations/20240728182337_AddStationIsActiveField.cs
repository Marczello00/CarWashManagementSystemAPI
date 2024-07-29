using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarWashManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddStationIsActiveField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsExcluded",
                table: "Stations",
                newName: "IsExcludedFromSchedule");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Stations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Stations");

            migrationBuilder.RenameColumn(
                name: "IsExcludedFromSchedule",
                table: "Stations",
                newName: "IsExcluded");
        }
    }
}
