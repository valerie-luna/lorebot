using static Weather.WeatherEnum;
using static Weather.CloudLevel;
using Weather.Context;

namespace Weather
{
    public static class WeatherGenerator
    {
        public static WeatherClass Generate(WeatherSettingsEntity settings, Random rand, double? setManaLevel)
        {
            if (setManaLevel > 100 || setManaLevel < 0)
                throw new InvalidOperationException("Invalid mana level");
            var manaLevel = setManaLevel ?? Math.Clamp(settings.ManaLevel.Generate(rand), 0, 100);
            WeatherValues wv = new(
                temperature: settings.Temperature.Generate(rand, manaLevel),
                airPressure: settings.AirPressure.Generate(rand, manaLevel),
                airQuality: Math.Max(0, settings.AirQuality.Generate(rand, manaLevel)),
                windspeed: Math.Max(0, settings.Windspeed.Generate(rand, manaLevel)),
                humidity: Math.Clamp(settings.Humidity.Generate(rand, manaLevel), 0, 100),
                manaLevel: manaLevel
            );
            List<string> customEvents = new();
            var (weather, clouds) = GenerateWeather(in wv, settings.CloudyMod, rand);
            foreach (var calc in WeatherEvents.Get(settings.CanonicalName))
            {
                string? custom;
                (weather, clouds, custom) = calc(in wv, weather, clouds, rand);
                if (custom is not null) customEvents.Add(custom);
            }
            return new WeatherClass(settings.CanonicalName, wv.temperature, wv.airPressure, wv.windspeed,
                wv.humidity, wv.manaLevel, wv.airQuality, weather, clouds, customEvents.ToArray());
        }

        private static (WeatherEnum, CloudLevel) GenerateWeather(in WeatherValues wv, NormalDistribution cloudMod, Random rand)
        {
            CloudLevel cloud = GenerateClouds(wv.airPressure, wv.humidity, cloudMod, wv.manaLevel, rand);
            WeatherEnum weather = RainSnowCalculation(wv.temperature, wv.airPressure, wv.humidity,
                wv.manaLevel, rand);
            if (weather is LightSnow or LightRain && cloud is SkyClear)
                if (wv.windspeed < 20)
                    cloud = Trace;
            if (weather is Snow or Rain && cloud is SkyClear or Trace or Scattered)
                if (wv.windspeed < 40)
                cloud = Broken;
            if (weather is HeavySnow or HeavyRain)
                if (wv.windspeed < 60)
                    cloud = Overcast;
            return (weather, cloud);
        }

        private static WeatherEnum RainSnowCalculation(double temperature, double airPressure, double humidity,
            double manaLevel, Random rand)
        {
            var airPressureModifier = 1013.25 - airPressure;
            var dist = new NormalDistribution(airPressureModifier, 10);
            double rainChance = humidity + dist.Generate(rand, manaLevel);
            bool snow = temperature < 4;
            return rainChance switch
            {
                > 130 => snow ? Blizzard : Thunderstorm,
                > 120 => Hailstorm,
                > 100 => snow ? HeavySnow : HeavyRain,
                > 75 => snow ? Snow : Rain,
                > 50 => snow ? LightSnow : LightRain,
                _ => Clear
            };
        }

        private static CloudLevel GenerateClouds(double AirPressure, double Humidity, 
            NormalDistribution cloudMod, double manaLevel, Random rand)
        {
            var airPressureModifier = 1013.25 - AirPressure;
            var dist = new NormalDistribution(airPressureModifier, 10);
            double aerialCloudHumidity = Humidity + dist.Generate(rand, manaLevel) + cloudMod.Generate(rand);
            return aerialCloudHumidity switch
            {
                > 130 => Overcast,
                > 90 => Broken,
                > 60 => Scattered,
                > 0 => Trace,
                _ => SkyClear
            };
        }
    }
}