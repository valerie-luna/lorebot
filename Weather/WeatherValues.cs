namespace Weather
{
    public record struct WeatherValues(double temperature, double airPressure,
            double airQuality, double windspeed, double humidity, double manaLevel);
}