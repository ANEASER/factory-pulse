using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryPulse_Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSensorDataColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Humidity",
                table: "SensorData",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Pressure",
                table: "SensorData",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "RPM",
                table: "SensorData",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Humidity",
                table: "SensorData");

            migrationBuilder.DropColumn(
                name: "Pressure",
                table: "SensorData");

            migrationBuilder.DropColumn(
                name: "RPM",
                table: "SensorData");
        }
    }
}
