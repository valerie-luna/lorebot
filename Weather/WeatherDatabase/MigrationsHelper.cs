using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Weather.Context;

internal class MigrationsHelper : IDesignTimeDbContextFactory<WeatherContext>
{
    public WeatherContext CreateDbContext(string[] args)
    {
        var opt = new DbContextOptionsBuilder<WeatherContext>()
            .UseNpgsql("");

        return new WeatherContext(opt.Options);
    }
}