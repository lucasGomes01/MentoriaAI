using MassTransit;
using MentoriaAI.Contracts.Events;
using Microsoft.EntityFrameworkCore;

namespace MentoriaAI.Embeddings.Consumers
{
    public class MentorDeletadoConsumer : IConsumer<MentorDeletadoEvent>
    {
        private readonly EmbeddingsContext _context;

        public MentorDeletadoConsumer(EmbeddingsContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<MentorDeletadoEvent> context)
        {
            var msg = context.Message;
            Console.WriteLine($"[Worker] Recebido MentorDeletadoEvent: Id={msg.Id}");

            var embedding = await _context.MentorEmbeddings
                .FirstOrDefaultAsync(e => e.MentorId == msg.Id);

            if (embedding != null)
            {
                _context.MentorEmbeddings.Remove(embedding);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Embedding removido para MentorId={msg.Id}");
            }
            else
            {
                Console.WriteLine($"Nenhum embedding encontrado para MentorId={msg.Id}");
            }
        }
    }
}
