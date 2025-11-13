using MentoriaAI.BuscaSemantica.Repositories;
using System.Text;

namespace MentoriaAI.BuscaSemantica.Services
{
    public class BuscaSemanticaService
    {
        private readonly OpenAIEmbeddingService _openAI;
        private readonly OpenAIChatService _chat;

        private readonly IEmbeddingsRepository _repository;

        public BuscaSemanticaService(OpenAIEmbeddingService openAI,
                                     OpenAIChatService chat,

                                     IEmbeddingsRepository repository)
        {
            _openAI = openAI;
            _chat = chat;

            _repository = repository;
        }

        public async Task<string> BuscarMentoresAsync(string query, int top = 5)
        {
            try
            {
                var embeddingConsulta = await _openAI.CreateEmbeddingAsync(query);
                var mentores = await _repository.BuscarTopMentoresAsync(embeddingConsulta, top);

                if (!mentores.Any())
                    return "Nenhum mentor compatível.";

                StringBuilder resumo = new();
                resumo.AppendLine($"Consulta: {query}");
                resumo.AppendLine("Mentores encontrados:");

                foreach (var m in mentores)
                    resumo.AppendLine($"- {m.Nome}: {m.Descricao}");

                var resposta = await _chat.GerarRespostaNaturalAsync(resumo.ToString());

                return resposta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar resposta: {ex.Message}");
                return "Ocorreu um erro ao procurar um mentor.";
            }
        }
    }
}
