using Microsoft.EntityFrameworkCore;

namespace Weather.Context;

public class WeatherContext : DbContext
{
    public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
    {

    }

    public DbSet<WeatherNameEntity> Names { get; set; } = default!;
    public DbSet<WeatherSettingsEntity> WeatherSettings { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherNameEntity>(e => 
        {
            e.HasKey(e => e.Id);

            e.HasAlternateKey(e => e.Name);

            e.HasOne(e => e.Settings)
                .WithMany()
                .HasForeignKey(e => e.WeatherSettingsId);

            e.Property(e => e.Name)
                .IsRequired();

            e.Property(e => e.Id)
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<WeatherSettingsEntity>(e =>
        {
            e.HasKey(e => e.Id);

            e.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            e.OwnsOne(e => e.Temperature);
            e.OwnsOne(e => e.AirPressure);
            e.OwnsOne(e => e.Windspeed);
            e.OwnsOne(e => e.Humidity);
            e.OwnsOne(e => e.ManaLevel);
            e.OwnsOne(e => e.CloudyMod);
            e.OwnsOne(e => e.AirQuality);
        });

        SeedData.Seed(modelBuilder);
    }
}
