using MassTransit;
using MentoriaAI.Contracts.Events;
using MentoriaAI.Embeddings.Data;
using MentoriaAI.Embeddings.Services;
using Microsoft.EntityFrameworkCore;

namespace MentoriaAI.Embeddings.Consumers
{
    public class MentorAtualizadoConsumer : IConsumer<MentorAtualizadoEvent>
    {
        private readonly EmbeddingsContext _context;
        private readonly OpenAIEmbeddingService _openAI;

        public MentorAtualizadoConsumer(EmbeddingsContext context, OpenAIEmbeddingService openAI)
        {
            _context = context;
            _openAI = openAI;
        }

        public async Task Consume(ConsumeContext<MentorAtualizadoEvent> context)
        {
            var msg = context.Message;
            Console.WriteLine($"[Worker] Recebido MentorAtualizadoEvent: {msg.Nome}");

            var embeddingExistente = await _context.MentorEmbeddings
                .FirstOrDefaultAsync(e => e.Id == msg.Id);

            if (embeddingExistente is null)
            {
                Console.WriteLine($"Nenhum embedding existente para {msg.Nome}.");
                return;
            }

            var texto = $"{msg.Nome}. {msg.Area}. {msg.Tecnologias}. {msg.Descricao}";
            var novoEmbedding = await _openAI.CreateEmbeddingAsync(texto);

            embeddingExistente.Nome = msg.Nome;
            embeddingExistente.Descricao = msg.Descricao;
            embeddingExistente.Embedding = novoEmbedding;

            await _context.SaveChangesAsync();
            Console.WriteLine($"Embedding atualizado para {msg.Nome}");
        }
    }
}
