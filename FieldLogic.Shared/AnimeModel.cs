using System.Text.Json.Serialization;

namespace FieldLogic.Shared;

public class AnimeSearchResponse
{
    [JsonPropertyName("data")] public List<AnimeData> Data { get; set; } = [];
}

public class AnimeData
{
    [JsonPropertyName("mal_id")] public int MalId { get; set; } = 0;

    [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;

    [JsonPropertyName("title_english")] public string? TitleEnglish { get; set; } = string.Empty;

    [JsonPropertyName("synopsis")] public string Synopsis { get; set; } = string.Empty;

    [JsonPropertyName("images")] public Images Images { get; set; } = new();

    public string DisplayTitle => string.IsNullOrWhiteSpace(TitleEnglish) ? Title : TitleEnglish;

    [JsonPropertyName("episodes")] public int? Episodes { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }
}

public class Images
{
    [JsonPropertyName("jpg")] public Jpg Jpg { get; set; } = new();
}

public class Jpg
{
    [JsonPropertyName("image_url")] public string ImageUrl { get; set; } = string.Empty;
}