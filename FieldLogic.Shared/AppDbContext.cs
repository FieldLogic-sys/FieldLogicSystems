using Microsoft.EntityFrameworkCore;

namespace FieldLogic.Shared;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<MediaEntry> MediaEntries { get; set; }
    public DbSet<JournalEntry> JournalEntries { get; set; }
    // Gemini assisted in setting up a proper keystore
    // We REMOVE OnConfiguring because Program.cs handles the connection.
    // This makes the DbContext "Environment Agnostic" (it doesn't care where the DB is).
}