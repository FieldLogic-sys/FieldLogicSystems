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
            .Where(n => !n.IsDeleted)
            .Include(n => n.Tags)
            .Include(n => n.Sources)
            .OrderByDescending(n => n.Created)
            .ToListAsync<IntelligenceNote>();

    }

    public async Task UpdateNoteAsync(IntelligenceNote note)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        
        var exists = await db.IntelligenceNotes.AnyAsync(n => n.Id == note.Id);
        if (!exists)
        {
            throw new KeyNotFoundException($"Note with id {note.Id} not found");
        }
        
        // Updating the LastModified timestamp
        note.LastModified = DateTime.UtcNow;
        
        db.IntelligenceNotes.Update(note); 
        await db.SaveChangesAsync();

    }

    public async Task DeleteNoteAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var note = await db.IntelligenceNotes.FindAsync(id);

        if (note != null)
        {
            db.IntelligenceNotes.Remove(note);
            await db.SaveChangesAsync();
        }
    }


    public async Task<List<IntelligenceNote>> SearchNoteAsync(string searchTerm)
    {
        await using var db = await dbFactory.CreateDbContextAsync();


        return await db.IntelligenceNotes
            .Where(n => n.Title.Contains(searchTerm) || n.Content.Contains(searchTerm))
            .Include(n => n.Tags)
            .ToListAsync();
    }


    public async Task ArchiveNoteAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var note = await db.IntelligenceNotes.FindAsync(id);

        if (note != null)
        {
            note.Category = ParaCategory.Archive;
            note.LastModified = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
        
    }


    public async Task SoftDeleteNoteAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var note = await db.IntelligenceNotes.FindAsync(id);

        if (note != null)
        {
            note.IsDeleted = true;
            note.DeletedAt =  DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
    }
    
}