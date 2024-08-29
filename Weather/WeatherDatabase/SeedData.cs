using Microsoft.EntityFrameworkCore;

namespace Weather.Context;

public static class SeedData
{
    public static void Seed(ModelBuilder builder)
    {
        // Tromsø
        builder.Entity<WeatherSettingsEntity>().HasData(new { Id = 1 , CanonicalName = "Tromsø"});
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.Temperature).HasData(new { Mean = -3.0, StandardDeviation = 3.0, WeatherSettingsEntityId = 1 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.AirPressure).HasData(new { Mean = 1004.1, StandardDeviation = 10.0, WeatherSettingsEntityId = 1 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.Windspeed).HasData(new { Mean = 16.9, StandardDeviation = 10.0, WeatherSettingsEntityId = 1 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.Humidity).HasData(new { Mean = 84.0, StandardDeviation = 20.0, WeatherSettingsEntityId = 1 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.ManaLevel).HasData(new { Mean = 50.0, StandardDeviation = 30.0, WeatherSettingsEntityId = 1 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.CloudyMod).HasData(new { Mean = -15.0, StandardDeviation = 20.0, WeatherSettingsEntityId = 1 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.AirQuality).HasData(new { Mean = 15.0, StandardDeviation = 20.0, WeatherSettingsEntityId = 1 });
        

        builder.Entity<WeatherNameEntity>().HasData(
            new WeatherNameEntity { Id = 1, Name = "tromso", WeatherSettingsId = 1 },
            new WeatherNameEntity { Id = 2, Name = "tromsø", WeatherSettingsId = 1 }
        );

        // Melbourne
        builder.Entity<WeatherSettingsEntity>().HasData(new { Id = 2, CanonicalName = "Melbourne" });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.Temperature).HasData(new { Mean = 20.6, StandardDeviation = 7.0, WeatherSettingsEntityId = 2 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.AirPressure).HasData(new { Mean = 1016.1, StandardDeviation = 20.0, WeatherSettingsEntityId = 2 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.Windspeed).HasData(new { Mean = 9.0, StandardDeviation = 10.0, WeatherSettingsEntityId = 2 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.Humidity).HasData(new { Mean = 44.0, StandardDeviation = 20.0, WeatherSettingsEntityId = 2 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.ManaLevel).HasData(new { Mean = 30.0, StandardDeviation = 30.0, WeatherSettingsEntityId = 2 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.CloudyMod).HasData(new { Mean = 20.0, StandardDeviation = 20.0, WeatherSettingsEntityId = 2 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.AirQuality).HasData(new { Mean = 50.0, StandardDeviation = 45.0, WeatherSettingsEntityId = 2 });

        builder.Entity<WeatherNameEntity>().HasData(
            new WeatherNameEntity { Id = 3, Name = "melbourne", WeatherSettingsId = 2 }
        );

        // Seattle
        builder.Entity<WeatherSettingsEntity>().HasData(new { Id = 3, CanonicalName = "Seattle" });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.Temperature).HasData(new { Mean = 12.1, StandardDeviation = 4.3, WeatherSettingsEntityId = 3 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.AirPressure).HasData(new { Mean = 1017.1, StandardDeviation = 12.0, WeatherSettingsEntityId = 3 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.Windspeed).HasData(new { Mean = 14.2, StandardDeviation = 8.0, WeatherSettingsEntityId = 3 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.Humidity).HasData(new { Mean = 73.0, StandardDeviation = 10.0, WeatherSettingsEntityId = 3 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.ManaLevel).HasData(new { Mean = 20.0, StandardDeviation = 25.0, WeatherSettingsEntityId = 3 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.CloudyMod).HasData(new { Mean = 40.0, StandardDeviation = 20.0, WeatherSettingsEntityId = 3 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.AirQuality).HasData(new { Mean = 75.0, StandardDeviation = 75.0, WeatherSettingsEntityId = 3 });

        builder.Entity<WeatherNameEntity>().HasData(
            new WeatherNameEntity { Id = 4, Name = "seattle", WeatherSettingsId = 3 }
        );

        // Berlin
        builder.Entity<WeatherSettingsEntity>().HasData(new { Id = 4, CanonicalName = "Berlin" });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.Temperature).HasData(new { Mean = 13.0, StandardDeviation = 5.0, WeatherSettingsEntityId = 4 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.AirPressure).HasData(new { Mean = 1015.1, StandardDeviation = 15.0, WeatherSettingsEntityId = 4 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.Windspeed).HasData(new { Mean = 14.2, StandardDeviation = 5.0, WeatherSettingsEntityId = 4 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.Humidity).HasData(new { Mean = 87.0, StandardDeviation = 10.0, WeatherSettingsEntityId = 4 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.ManaLevel).HasData(new { Mean = 40.0, StandardDeviation = 25.0, WeatherSettingsEntityId = 4 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.CloudyMod).HasData(new { Mean = -5.0, StandardDeviation = 5.0, WeatherSettingsEntityId = 4 });
        builder.Entity<WeatherSettingsEntity>().OwnsOne(e => e.AirQuality).HasData(new { Mean = 65.0, StandardDeviation = 65.0, WeatherSettingsEntityId = 4 });

        builder.Entity<WeatherNameEntity>().HasData(
            new WeatherNameEntity { Id = 5, Name = "berlin", WeatherSettingsId = 4 }
        );
    }
}