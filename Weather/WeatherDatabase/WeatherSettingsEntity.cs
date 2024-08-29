namespace Weather.Context;

public class WeatherSettingsEntity
{
    public int Id { get; set; }
    public required string CanonicalName { get; set; }
    public required NormalDistribution Temperature { get; set; }
    public required NormalDistribution AirPressure { get; set; }
    public required NormalDistribution Windspeed { get; set; }
    public required NormalDistribution Humidity { get; set; }
    public required NormalDistribution ManaLevel { get; set; }
    public required NormalDistribution CloudyMod { get; set; }
    public required NormalDistribution AirQuality { get; set; }
}
