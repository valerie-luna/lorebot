using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Initiative.Database;

internal class MigrationsHelper : IDesignTimeDbContextFactory<InitiativeContext>
{
    public InitiativeContext CreateDbContext(string[] args)
    {
        var opt = new DbContextOptionsBuilder<InitiativeContext>()
            .UseNpgsql("");

        return new InitiativeContext(opt.Options);
    }
}