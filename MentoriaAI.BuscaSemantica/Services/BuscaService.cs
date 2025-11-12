using MentoriaAI.BuscaSemantica.Repositories;

namespace MentoriaAI.BuscaSemantica.Services
{
    public class BuscaSemanticaService
    {
        private readonly OpenAIEmbeddingService _openAI;
        private readonly IEmbeddingsRepository _repository;

        public BuscaSemanticaService(OpenAIEmbeddingService openAI, IEmbeddingsRepository repository)
        {
            _openAI = openAI;
            _repository = repository;
        }

        public async Task<IEnumerable<object>> BuscarMentoresAsync(string query, int top = 5)
        {
            var embeddingConsulta = await _openAI.CreateEmbeddingAsync(query);
            var mentores = await _repository.BuscarTopMentoresAsync(embeddingConsulta, top);

            return mentores.Select(m => new
            {
                m.Id,
                m.MentorId,
                m.Nome,
                m.Descricao
            });
        }
    }
}
