namespace FieldLogic.Shared.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Many-to-Many back-reference
    // Adding 'null!' tells Rider: "I know EF Core will fill this, don't worry about nulls."
    public List<IntelligenceNote> Notes { get; set; } = [];
}