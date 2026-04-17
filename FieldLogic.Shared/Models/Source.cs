namespace FieldLogic.Shared.Models;


public class Source
{
    public int Id { get; set; }
    public string Url { get; set;  } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    
    // The foreign key
    
    
    public int IntelligenceNoteId { get; set; }
    
    // The actual note object
    public IntelligenceNote IntelligenceNote { get; set; } = null!;


}