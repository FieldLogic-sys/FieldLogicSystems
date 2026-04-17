using FieldLogic.Shared.Models;
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
    public DbSet<IntelligenceNote> IntelligenceNotes { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<Tag> Tags { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<IntelligenceNote>()
            .HasMany(n=>n.Tags)
            .WithMany(t => t.Notes);
    }
}