using System;

namespace FieldLogic.Shared;

public class JournalEntry
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Content { get; set; } = string.Empty;
    public string Area { get; set; } = "Inbetriebnahme"; 
}