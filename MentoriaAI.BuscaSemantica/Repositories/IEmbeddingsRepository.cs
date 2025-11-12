using MentoriaAI.BuscaSemantica.Models;
using Pgvector;

namespace MentoriaAI.BuscaSemantica.Repositories
{
    public interface IEmbeddingsRepository
    {
        Task<IEnumerable<MentorEmbedding>> BuscarTopMentoresAsync(Vector vetorConsulta, int top = 5);
    }
}
