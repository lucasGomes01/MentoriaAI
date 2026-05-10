using MentoriaAI.BuscaSemantica.DTOs;
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

        public async Task<BuscaMentoresDto> BuscarMentoresAsync(string query, int top = 5)
        {
            var result = new BuscaMentoresDto();

            var embeddingConsulta = await _openAI.CreateEmbeddingAsync(query);
            var mentores = await _repository.BuscarTopMentoresAsync(embeddingConsulta, top);

            if (!mentores.Any())
            {
                result.Resumo = "Nenhum mentor compatível.";
                return result;
            }

            var contexto = new StringBuilder();
            contexto.AppendLine($"Consulta: {query}");
            contexto.AppendLine("Mentores encontrados:");

            foreach (var m in mentores)
                contexto.AppendLine($"- ID:{m.Id} | {m.Nome}: {m.Descricao}");

            var respostaIA = await _chat.GerarRespostaNaturalAsync(contexto.ToString());

            var mentoresSugeridos = mentores
                .Join(
                    respostaIA.Mentores,
                    m => m.Id,
                    ia => ia.Id,
                    (m, ia) => new MentorSemanticoDto
                    {
                        Id = m.Id,
                        Nome = m.Nome,
                        Descricao = m.Descricao,
                        Motivo = ia.Motivo
                    }
                )
                .Take(2)
                .ToList();

            result.Resumo = respostaIA.Resumo;
            result.Mentores = mentoresSugeridos;

            return result;
        }
    }
}
