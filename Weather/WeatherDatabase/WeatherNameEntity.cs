namespace Weather.Context;

public class WeatherNameEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public WeatherSettingsEntity Settings { get; set; } = default!;
    public int WeatherSettingsId { get; set; }
}
