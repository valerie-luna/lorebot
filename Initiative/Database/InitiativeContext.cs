using Microsoft.EntityFrameworkCore;

namespace Initiative.Database;

public class InitiativeContext : DbContext
{
    public InitiativeContext(DbContextOptions<InitiativeContext> options)
        : base(options)
    {}

    internal DbSet<InitiativeEntity> Initiatives { get; set; } = default!;
    internal DbSet<LastRollEntity> LastRolls { get; set; } = default!;
    internal DbSet<ServerSettingsEntity> ServerSettings { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InitiativeEntity>(e =>
        {
            e.HasKey(e => e.Id);

            e.HasAlternateKey(e => new { e.ChannelId, e.Name });

            e.Property(e => e.Name)
                .HasMaxLength(256)
                .IsRequired();

            e.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            e.HasOne(e => e.ServerSettings)
                .WithMany()
                .HasForeignKey(e => e.ServerId);
        });

        modelBuilder.Entity<ServerSettingsEntity>(e => 
        {
            e.HasKey(e => e.ServerId);
        });

        modelBuilder.Entity<LastRollEntity>(e => 
        {
            e.HasKey(e => e.Id);

            e.HasAlternateKey(e => new { e.UserId, e.ChannelId });

            e.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            e.Property(e => e.Name)
                .HasMaxLength(256)
                .IsRequired();

            e.Property(e => e.Roll)
                .HasMaxLength(256)
                .IsRequired();             
        });
    }
}
