using System.ComponentModel.DataAnnotations;

namespace FieldLogic.Shared.Models;

public class IntelligenceNote
{
    [Key] 
    public int Id { get; set; }

    [Required] 
    [StringLength(200)] 
    public string Title { get; set; } = string.Empty;
    
    
    public string Content { get; set; } = string.Empty;

    public NoteType Type { get; set; } = NoteType.Technical;
    public ParaCategory Category { get; set; } = ParaCategory.Resource;
    
    
    // Timestamp
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
    
    
    // Navigation Properties (RDBMS Links)
    public List<Source> Sources
    {
        get;
        set;

    } = [];
    
    
    public List<Tag> Tags {get;set;} = [];


}