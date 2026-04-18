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
        
        var seedDate = new DateTime(2026, 04, 18, 0, 0, 0, DateTimeKind.Utc);
        
        modelBuilder.Entity<IntelligenceNote>().HasData(
            new IntelligenceNote
            {
                Id = 1,
                Title = "Gilbarco Encore 700S Pinout",
                Content = "Focus on the M7 interface for FlexPay IV integration. Ensure ground is common.",
                Type = NoteType.Technical,
                Category = ParaCategory.Resource,
                Created = seedDate,
                LastModified = seedDate
            },
            new IntelligenceNote
            {
                Id = 2,
                Title = "C# .NET 10 Collections",
                Content = "Use collection expressions [] instead of new() for better performance and readability.",
                Type = NoteType.Research,
                Category = ParaCategory.Area,
                Created = seedDate,
                LastModified = seedDate
            }
        );
    }
}