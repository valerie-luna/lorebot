using static Weather.WeatherEnum;

namespace Weather
{
    public static class WeatherEvents
    {
        public static WeatherCalculator[] Get(string name)
        {
            return name switch
            {
                "Tromsø" => GenTromsøWeather(),
                "Melbourne" => GenMelbourneWeather(),
                "Seattle" => GenSeattleWeather(),
                "Berlin" => GenBerlinWeather(),
                _ => throw new InvalidOperationException()
            };
        }
        private static WeatherCalculator[] GenTromsøWeather()
        {
            return new[]
            {
                FogRisk(),
                BloodRainRisk(95),
                ManaStormRisk(90),
                AuroraBorealisRisk(75),
            };
        }

        private static WeatherCalculator[] GenMelbourneWeather()
        {
            return new[]
            {
                FogRisk(),
                ManaStormRisk(85),
                WildfireRisk()
            };
        }

        private static WeatherCalculator[] GenSeattleWeather()
        {
            return new[]
            {
                FogRisk(),
                ManaStormRisk(90),
                AcidRainRisk(),
                SmogRisk(),
            };
        }

        private static WeatherCalculator[] GenBerlinWeather()
        {
            return new[]
            {
                FogRisk(),
                SmogRisk(),
                DragonRisk(),
            };
        }

        private record struct Requirements(WeatherEnum? weather, CloudLevel? cloud);
        private record struct Replacements(WeatherEnum? weather, CloudLevel? cloud, string? custevent);

        private static WeatherCalculator GenericRisk(Requirements req, Replacements rep, 
            params Func<WeatherValues, WeatherEnum, CloudLevel, double>[] functions)
        {
            return (in WeatherValues wv, WeatherEnum w, CloudLevel c, Random rand) =>
            {
                if ((req.weather is null || w.Matches(req.weather.Value))
                    && (req.cloud is null || c.Matches(req.cloud.Value)))
                {
                    double chance = 0;
                    foreach (var f in functions)
                    {
                        chance += f(wv, w, c);
                    }
                    if (rand.NextDouble() < chance)
                    {
                        return new WeatherUpdate(
                            rep.weather ?? w,
                            rep.cloud ?? c,
                            rep.custevent
                        );
                    }
                }
                return new (w, c, null);
            };
        }

        private static WeatherCalculator ManaStormRisk(double minimumValue)
        {
            return GenericRisk(default, new (ManaStorm, null, null), (wv, w, c) =>
            {
                if (wv.manaLevel > minimumValue)
                    return (wv.manaLevel - minimumValue) / (120 - minimumValue);
                return 0;
            });
        }

        private static WeatherCalculator AuroraBorealisRisk(double minimumValue)
        {
            return GenericRisk(default, new(null, null, "The Aurora Borealis fills the night sky."), 
                (wv, w, c) =>
            {
                if (wv.manaLevel > minimumValue)
                    return (wv.manaLevel - minimumValue) / (100 - minimumValue);
                return 0;
            });
        }

        private static WeatherCalculator AcidRainRisk()
        {
            return GenericRisk(new(RainyFlag, null), new (AcidRain, null, null), (wv, w, c) =>
            {
                if (wv.airQuality > 150)
                    return 0.7*((wv.airQuality - 100) / 200);
                return 0;
            });
        }

        private static WeatherCalculator DragonRisk()
        {
            return GenericRisk(default, new (null, null, "You can see the silouette of a dragon flying in the sky."), 
            (wv, w, c) =>
            {
                return wv.manaLevel > 20 ? 0.05 : 0;
            });
        }

        private static WeatherCalculator BloodRainRisk(double minimumValue)
        {
            return GenericRisk(new(RainyFlag, null), new (BloodRain, null, null), (wv, w, c) =>
            {
                if (wv.manaLevel > minimumValue)
                    return (wv.manaLevel - minimumValue) / (100 - minimumValue);
                return 0;
            });
        }

        private static WeatherCalculator WildfireRisk()
        {
            return GenericRisk(new (Clear, null), new (Wildfire, null, null), 
                (wv, w, c) => {
                    if (wv.humidity < 40)
                        return 0.3*((wv.humidity - 60) / 40);
                    return 0;
                }, (wv, w, c) => {
                    if (wv.temperature > 25)
                        return 0.5*((wv.temperature - 25) / 15);
                    return 0;
                }, (wv, w, c) => {
                    if (wv.windspeed > 25)
                        return 0.3*((wv.windspeed - 25) / 40);
                    return 0;
            });
        }

        private static WeatherCalculator FogRisk()
        {
            return GenericRisk(new (Clear, null), new (Foggy, null, null),
                (wv, w, c) => {
                    if (wv.humidity > 60)
                        return 0.5*((wv.humidity - 60) / 40);
                    return 0;
                }, (wv, w, c) => {
                    if (wv.airPressure < 1000)
                        return 0.5*((1000 - wv.airPressure) / 30);
                    return 0;
                }
            );
        }

        private static WeatherCalculator SmogRisk()
        {
            return GenericRisk(new (Clear, null), new (Smoggy, null, null),
                (wv, w, c) => {
                    if (wv.airQuality > 150)
                        return 0.7*((wv.airQuality - 100) / 200);
                    return 0;
                }, (wv, w, c) => {
                    if (wv.airPressure < 1000)
                        return 0.3*((1000 - wv.airPressure) / 30);
                    return 0;
                }
            );
        }
    }
}