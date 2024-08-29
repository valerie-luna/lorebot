namespace Weather
{
    public record WeatherUpdate(WeatherEnum weather, CloudLevel cloud, string? Custom);
}