using MentoriaAI.BuscaSemantica.Contracts;
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

        public async Task<RespostaIA> GerarRespostaNaturalAsync(string prompt)
        {
            try
            {

                var body = new
                {
                    model = "gpt-4o-mini",
                    response_format = new { type = "json_object" },
                    messages = new[]
                    {
                        new { role = "system", content = @"
                            Você deve responder APENAS em JSON válido no formato:

                            {
                              ""Mentores"": [
                                { ""Id"": number, ""Nome"": ""string"", ""Motivo"": ""string"" }
                              ],
                              ""Resumo"": ""string""
                            }

                            Não escreva nenhum texto fora do JSON. Retorne somente 2 mentores que sejam os mais relevantes para 
                            a consulta do usuário. O campo 'motivo' deve conter uma breve explicação de por que aquele mentor 
                            é relevante para a consulta. O campo 'resumo' deve conter um resumo geral da resposta, destacando os pontos 
                            mais importantes.
                        "
                        },
                        new { role = "user", content = prompt }
                    }
                };

                var response = await _http.PostAsJsonAsync(
                            "https://api.openai.com/v1/chat/completions",
                            body
                        );

                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                var content = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (string.IsNullOrWhiteSpace(content))
                    throw new Exception("Resposta vazia da OpenAI.");

                var resultado = JsonSerializer.Deserialize<RespostaIA>(content);

                if (resultado == null || resultado.Mentores == null)
                    throw new Exception("JSON inválido retornado pela IA.");

                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar resposta: {ex.Message}");
                throw;
            }
        }
    }
}
