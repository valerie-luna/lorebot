using Microsoft.EntityFrameworkCore;

namespace Lore.Music;

public class MusicContext : DbContext
{
    public MusicContext(DbContextOptions<MusicContext> options)
        : base(options)
    {}

    internal DbSet<MusicEntryEntity> Music { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MusicEntryEntity>(e => 
        {
            e.HasKey(e => e.Id);

            e.HasAlternateKey(e => new { e.ServerId, e.Name });

            e.Property(e => e.Name)
                .HasMaxLength(256)
                .IsRequired();

            e.Property(e => e.Id)
                .ValueGeneratedOnAdd();
        });
    }
}
