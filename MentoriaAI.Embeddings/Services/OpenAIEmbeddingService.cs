using System.Net.Http.Json;
using System.Text.Json;
using Pgvector;

namespace MentoriaAI.Embeddings.Services;

public class OpenAIEmbeddingService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public OpenAIEmbeddingService(IConfiguration config)
    {
        _http = new HttpClient();
        _apiKey = config["OpenAI:ApiKey"] ?? throw new Exception("OpenAI API key não configurada.");
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<Vector> CreateEmbeddingAsync(string text)
    {
        try
        {
            var payload = new { model = "text-embedding-3-small", input = text };

            var response = await _http.PostAsJsonAsync("https://api.openai.com/v1/embeddings", payload);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            var embeddingArray = doc.RootElement.GetProperty("data")[0]
                           .GetProperty("embedding")
                           .EnumerateArray()
                           .Select(e => e.GetSingle())
                           .ToArray();

            return new Vector(embeddingArray);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao criar embedding: {ex.Message}");
            throw;
        }
    }
}