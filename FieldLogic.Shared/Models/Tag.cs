// The many sides of the relationship in the db



namespace FieldLogic.Shared.Models;

public class Tag
{
    public int Id { get; set;  }
    public string Name { get; set; } = string.Empty;
    
    // The many-to-many tag links back to many notes that use the tag(s)


    public List<IntelligenceNote> Notes { get; set; } = [];
}