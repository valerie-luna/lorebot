using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Weather.WeatherDatabase.Migrations
{
    /// <inheritdoc />
    public partial class PostgresInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanonicalName = table.Column<string>(type: "text", nullable: false),
                    Temperature_Mean = table.Column<double>(type: "double precision", nullable: false),
                    Temperature_StandardDeviation = table.Column<double>(type: "double precision", nullable: false),
                    AirPressure_Mean = table.Column<double>(type: "double precision", nullable: false),
                    AirPressure_StandardDeviation = table.Column<double>(type: "double precision", nullable: false),
                    Windspeed_Mean = table.Column<double>(type: "double precision", nullable: false),
                    Windspeed_StandardDeviation = table.Column<double>(type: "double precision", nullable: false),
                    Humidity_Mean = table.Column<double>(type: "double precision", nullable: false),
                    Humidity_StandardDeviation = table.Column<double>(type: "double precision", nullable: false),
                    ManaLevel_Mean = table.Column<double>(type: "double precision", nullable: false),
                    ManaLevel_StandardDeviation = table.Column<double>(type: "double precision", nullable: false),
                    CloudyMod_Mean = table.Column<double>(type: "double precision", nullable: false),
                    CloudyMod_StandardDeviation = table.Column<double>(type: "double precision", nullable: false),
                    AirQuality_Mean = table.Column<double>(type: "double precision", nullable: false),
                    AirQuality_StandardDeviation = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Names",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    WeatherSettingsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Names", x => x.Id);
                    table.UniqueConstraint("AK_Names_Name", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Names_WeatherSettings_WeatherSettingsId",
                        column: x => x.WeatherSettingsId,
                        principalTable: "WeatherSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "WeatherSettings",
                columns: new[] { "Id", "CanonicalName", "AirPressure_Mean", "AirPressure_StandardDeviation", "AirQuality_Mean", "AirQuality_StandardDeviation", "CloudyMod_Mean", "CloudyMod_StandardDeviation", "Humidity_Mean", "Humidity_StandardDeviation", "ManaLevel_Mean", "ManaLevel_StandardDeviation", "Temperature_Mean", "Temperature_StandardDeviation", "Windspeed_Mean", "Windspeed_StandardDeviation" },
                values: new object[,]
                {
                    { 1, "Tromsø", 1004.1, 10.0, 15.0, 20.0, -15.0, 20.0, 84.0, 20.0, 50.0, 30.0, -3.0, 3.0, 16.899999999999999, 10.0 },
                    { 2, "Melbourne", 1016.1, 20.0, 50.0, 45.0, 20.0, 20.0, 44.0, 20.0, 30.0, 30.0, 20.600000000000001, 7.0, 9.0, 10.0 },
                    { 3, "Seattle", 1017.1, 12.0, 75.0, 75.0, 40.0, 20.0, 73.0, 10.0, 20.0, 25.0, 12.1, 4.2999999999999998, 14.199999999999999, 8.0 },
                    { 4, "Berlin", 1015.1, 15.0, 65.0, 65.0, -5.0, 5.0, 87.0, 10.0, 40.0, 25.0, 13.0, 5.0, 14.199999999999999, 5.0 }
                });

            migrationBuilder.InsertData(
                table: "Names",
                columns: new[] { "Id", "Name", "WeatherSettingsId" },
                values: new object[,]
                {
                    { 1, "tromso", 1 },
                    { 2, "tromsø", 1 },
                    { 3, "melbourne", 2 },
                    { 4, "seattle", 3 },
                    { 5, "berlin", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Names_WeatherSettingsId",
                table: "Names",
                column: "WeatherSettingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Names");

            migrationBuilder.DropTable(
                name: "WeatherSettings");
        }
    }
}
