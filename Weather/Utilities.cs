namespace Weather
{
    public static class Utilities
    {
        public static string ToPretty(this CloudLevel clouds)
        {
            return clouds switch
            {
                CloudLevel.SkyClear => "Clear Skies",
                CloudLevel.Trace => "Mostly Sunny",
                CloudLevel.Scattered => "Partly Cloudy",
                CloudLevel.Broken => "Mostly Cloudy",
                CloudLevel.Overcast => "Overcast",
                _ => clouds.ToString(),
            };
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
            where TKey : notnull
        {
            return enumerable.ToDictionary(t => t.Key, t => t.Value);
        }

        public static string ToPretty(this WeatherEnum weather)
        {
            return weather switch
            {
                WeatherEnum.LightRain => "Light Rain",
                WeatherEnum.LightSnow => "Light Snow",
                WeatherEnum.HeavySnow => "Heavy Snow",
                WeatherEnum.HeavyRain => "Heavy Rain",
                WeatherEnum.DustStorm => "Dust Storm",
                WeatherEnum.ManaStorm => "Mana Storm",
                WeatherEnum.AcidRain => "Acid Rain",
                WeatherEnum.BloodRain => "Blood Rain",
                _ => weather.ToString(),
            };
        }

        public static string ToPretty(this AirQualityEnum airq)
        {
            return airq switch
            {
                AirQualityEnum.Good => "Good",
                AirQualityEnum.Moderate => "Moderate",
                AirQualityEnum.UnhealthyForSensitive => "Unhealthy for Sensitive Groups",
                AirQualityEnum.Unhealthy => "Unhealthy",
                AirQualityEnum.VeryUnhealthy => "Very Unhealthy",
                AirQualityEnum.Hazardous => "Hazardous",
                AirQualityEnum.Extreme => "Extreme",
                _ => throw new NotImplementedException(),
            };
        }

        public static bool Matches<T>(this T enumToCheck, T test)
            where T : struct, Enum
        {
            if (test.ToString().Contains("Flag"))
                return enumToCheck.HasFlag(test);
            else
                return enumToCheck.Equals(test);
        }
    }
}