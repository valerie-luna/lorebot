namespace Weather
{
    public enum WeatherEnum
    {
        Unknown = 0,
        Clear = 1,
        Snow = 2,
        Rain = 3 | RainyFlag,
        HeavySnow = 4 | SnowyFlag,
        HeavyRain = 5 | RainyFlag,
        LightSnow = 6 | SnowyFlag,
        LightRain = 7 | RainyFlag,

        Foggy = 8,
        DustStorm = 9,
        ManaStorm = 10,
        Tornado = 11,
        Wildfire = 12,

        AcidRain = 13 | RainyFlag,
        Flooding = 14 | RainyFlag,
        Thunderstorm = 15 | RainyFlag,
        Blizzard = 16 | SnowyFlag,
        Hailstorm = 17 | PrecipitationFlag,
        BloodRain = 18 | RainyFlag,

        Smoggy = 19,
        
        RainyFlag = 0b10000000,
        SnowyFlag = 0b01000000,
        PrecipitationFlag = RainyFlag | SnowyFlag
    }
}