using System.Text;

namespace Weather
{
    public class WeatherClass
    {
        public string Name { get; }
        public double Temperature { get; }
        public double AirPressure { get; }
        public double Windspeed { get; }
        public double Humidity { get; }
        public double ManaLevel { get; }
        public WeatherEnum Weather { get; }
        public CloudLevel CloudLevel { get; }
        public double AQI { get; }
        public string[] CustomEvents { get; }
        public AirQualityEnum AirQuality => AQI switch
        {
            < 50 => AirQualityEnum.Good,
            < 100 => AirQualityEnum.Moderate,
            < 150 => AirQualityEnum.UnhealthyForSensitive,
            < 200 => AirQualityEnum.Unhealthy,
            < 300 => AirQualityEnum.VeryUnhealthy,
            < 500 => AirQualityEnum.Hazardous,
            _ => AirQualityEnum.Extreme
        };

        public WeatherClass(string Name, double temperature, double airPressure, double windspeed, 
            double humidity, double manaLevel, double AQI,
            WeatherEnum weather, CloudLevel clouds, params string[] CustomEvents)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException($"'{nameof(Name)}' cannot be null or whitespace.", nameof(Name));

            if (temperature < -273.15)
                throw new ArgumentException("Temperature below absolute zero", nameof(temperature));
            if (airPressure < 0)
                throw new ArgumentException("Can't have an air pressure below 0.", nameof(airPressure));
            if (windspeed < 0)
                throw new ArgumentException("Can't have a windspeed below 0.", nameof(windspeed));
            if (humidity < 0 || humidity > 100)
                throw new ArgumentException("Humidity is a percentage between 0 and 100%", nameof(humidity));
            if (manaLevel < 0 || manaLevel > 100)
                throw new ArgumentException("Mana level is a percentage between 0 and 100%", nameof(manaLevel));
            if (AQI < 0)
                throw new ArgumentException("Air quality index cannot be lower than 0.", nameof(AQI));
            this.Name = Name;
            this.Temperature = temperature;
            this.AirPressure = airPressure;
            this.Windspeed = windspeed;
            this.Humidity = humidity;
            this.ManaLevel = manaLevel;
            this.Weather = weather;
            this.CloudLevel = clouds;
            this.AQI = AQI;
            this.CustomEvents = CustomEvents;
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            builder.AppendLine($"__**Weather for {Name}**__");
            builder.AppendLine($"**Temperature:** {Temperature:0.0} C");
            builder.AppendLine($"**Air Pressure:** {AirPressure:0.0} hPa");
            builder.AppendLine($"**Windspeed:** {Windspeed:0.0} kph");
            builder.AppendLine($"**Humidity:** {Humidity:0.0}%");
            builder.AppendLine($"**Mana level:** {ManaLevel:0.0}%");
            builder.AppendLine($"**Air Quality:** {AirQuality.ToPretty()} ({AQI:0.0})");
            builder.AppendLine($"**Cloud Cover:** {CloudLevel.ToPretty()}");
            builder.AppendLine($"**Weather:** {Weather.ToPretty()}");
            if (CustomEvents.Any())
            {
                builder.AppendLine("**Special Events:**");
                foreach (var ev in CustomEvents)
                    builder.AppendLine(ev);
            }
            return builder.ToString();
        }
    }
}