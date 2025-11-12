using MentoriaAI.BuscaSemantica.Data;
using MentoriaAI.BuscaSemantica.Models;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace MentoriaAI.BuscaSemantica.Repositories
{
    public class EmbeddingsRepository : IEmbeddingsRepository
    {
        private readonly EmbeddingsContext _context;

        public EmbeddingsRepository(EmbeddingsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MentorEmbedding>> BuscarTopMentoresAsync(Vector vetorConsulta, int top = 5)
        {
            return await _context.MentorEmbeddings
                .OrderBy(e => e.Embedding.CosineDistance(vetorConsulta))
                .Take(top)
                .ToListAsync();
        }
    }
}
