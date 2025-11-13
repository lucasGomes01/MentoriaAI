using System.Net.Http.Json;
using System.Text.Json;

namespace MentoriaAI.BuscaSemantica.Services
{
    public class OpenAIChatService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public OpenAIChatService(IConfiguration config)
        {
            _http = new HttpClient();
            _apiKey = config["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> GerarRespostaNaturalAsync(string prompt)
        {
            try
            {

                var body = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                        new { role = "system", content = @"Você é um assistente que ajuda usuários a encontrar mentores ideais. 
                            Sempre responda no formato abaixo, sem inventar informações inexistentes, 
                            repassando somente os usuarios que fazem sentido, você não deve comentar sobre os mentores que você não recomendar:
                            
                            Mentores recomendados:
                            1. [Nome] - [Motivo]
                            2. [Nome] - [Motivo]
                            
                            Resumo final: [breve texto explicando por que eles são boas opções].
                            no final respode com isso: \\r { lista com os ids que recomendar }"
                },
                    new { role = "user", content = prompt }
                }
                };

                var response = await _http.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", body);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                return doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar resposta: {ex.Message}");
                throw;
            }
        }
    }
}
