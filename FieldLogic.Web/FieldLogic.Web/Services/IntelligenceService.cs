using FieldLogic.Shared;
using FieldLogic.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace FieldLogic.Web.Services;

public class IntelligenceService(IDbContextFactory<AppDbContext> dbFactory)
{
    // Create: Adding a new note to the Intelligence Unit

    public async Task CreateNoteAsync(IntelligenceNote note)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        
        //  Ensure timestamps are set 
        note.Created = DateTime.UtcNow;
        note.LastModified = DateTime.UtcNow;

        db.IntelligenceNotes.Add(note);
        await db.SaveChangesAsync();

    }
    
    // Read: Fetching all notes with their Tags and Sources included


    public async Task<List<IntelligenceNote>> GetAllNotesAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        
        // Eager Loading by pulling the related RDBMS data in one go.
        
        
        return await db.IntelligenceNotes
            .Include(n => n.Tags)
            .Include(n => n.Sources)
            .OrderByDescending(n => n.Created)
            .ToListAsync<IntelligenceNote>();

    }
    
}