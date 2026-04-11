using System.Text.Json.Serialization;

namespace FieldLogic.Shared;

public class AnimeSearchResponse
{
    public List<AnimeData> Data { get; set; } = new();
}

public class AnimeData
{
    [JsonPropertyName("mail_id")]
    public int MalId { get; set; } = new();
    
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    [JsonPropertyName("title_english")]
    public string TitleEnglish { get; set; } = string.Empty;
    
    [JsonPropertyName("synopsis")]
    public string Synopsis { get; set; } = string.Empty;
    [JsonPropertyName("images")]
    public Images Images { get; set; } = new();

    public string DisplayTitle => string.IsNullOrWhiteSpace(TitleEnglish) ? Title : TitleEnglish;
}

public class Images
{
    public Jpg Jpg { get; set; } = new();
}

public class Jpg
{
    public string ImageUrl { get; set; } = string.Empty;
}
