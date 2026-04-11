using System.Net.Http.Json;
using FieldLogic.Shared;


namespace FieldLogic.Web.Services;

public class JikanService
{
    private readonly HttpClient _httpClient;

    public JikanService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "FieldLogicSys-App");
    }

    public async Task<List<AnimeData>> SearchAnimeAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return new();

        var url = $"https://api.jikan.moe/v4/anime?q={Uri.EscapeDataString(query)}&limit=5";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<AnimeSearchResponse>(url);
            return response?.Data ?? new List<AnimeData>();
        }

        catch (Exception ex)
        {
            Console.WriteLine($"API Error: {ex.Message}");
            return new();
        }
    }
}

