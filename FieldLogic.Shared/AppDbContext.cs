using Microsoft.EntityFrameworkCore;

namespace FieldLogic.Shared;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<MediaEntry> MediaEntries { get; set; }
    public DbSet<JournalEntry> JournalEntries { get; set; } // This will turn green now

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        }
    }
}