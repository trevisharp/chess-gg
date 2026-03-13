using System.Net;

namespace ChessGG.Infrastructure;

public class ChessService
{
    const string publicChessAPI = "https://api.chess.com/";
    readonly HttpClient client;

    public ChessService()
    {
        client = new () {
            BaseAddress = new Uri(publicChessAPI)
        };

        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "ChessGG/1.0 (contact: leonardotrevisansilio@gmail.com)"
        );
        client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    }

    public async Task<T?> Get<T>(string player, string path = "")
    {
        var response = await client.GetAsync($"pub/player/{player}/{path}");

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
            throw new Exception("Too many requests to chess.com API.");
        
        if (response.StatusCode == HttpStatusCode.NotFound)
            return default;

        if (response.StatusCode != HttpStatusCode.OK)
            throw new Exception($"chess.com API responses with error ({response.StatusCode}).");

        return await response.Content.ReadFromJsonAsync<T>()
            ?? throw new Exception("chess.com returns with a empty response.");
    }
}