namespace FieldLogic.Shared;

public class MediaEntry
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? EnglishTitle { get; set; }
    public string? ImageUrl { get; set; }
    public int? TotalEpisodes { get;
        set => field = value < 0 ? 0 : value;
    }
    public string Type { get; set; } = "Anime";

    // The C# 14 'field' keyword I am learning more about
    public int Progress
    {
        get;
        set => field = value < 0 ? 0 : value;
    }

    public string Status { get; set; } = "Watching";
}