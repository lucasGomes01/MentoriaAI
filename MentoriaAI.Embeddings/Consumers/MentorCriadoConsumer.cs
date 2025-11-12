using MassTransit;
using MentoriaAI.Embeddings.Models;
using MentoriaAI.Embeddings.Services;
using MentoriaAI.Contracts.Events;

namespace MentoriaAI.Embeddings.Consumers
{
    public class MentorCriadoConsumer : IConsumer<MentorCriadoEvent>
    {
        private readonly EmbeddingsContext _context;
        private readonly OpenAIEmbeddingService _openAI;

        public MentorCriadoConsumer(EmbeddingsContext context, OpenAIEmbeddingService openAI)
        {
            _context = context;
            _openAI = openAI;
        }

        public async Task Consume(ConsumeContext<MentorCriadoEvent> context)
        {
            var msg = context.Message;
            var texto = $"{msg.Nome}. {msg.Area}. {msg.Tecnologias}. {msg.Descricao}";
            Console.WriteLine($"[Worker] Gerando embedding para {msg.Nome}...");

            var embedding = await _openAI.CreateEmbeddingAsync(texto);

            _context.MentorEmbeddings.Add(new MentorEmbedding
            {
                MentorId = msg.Id,
                Nome = msg.Nome,
                Descricao = msg.Descricao,
                Embedding = embedding
            });

            await _context.SaveChangesAsync();
            Console.WriteLine($"[Worker] Embedding salvo para {msg.Nome}");
        }
    }
}
