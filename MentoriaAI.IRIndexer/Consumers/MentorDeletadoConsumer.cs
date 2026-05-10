using MassTransit;
using MentoriaAI.Contracts.Events;
using MentoriaAI.IRIndexer.Services;

namespace MentoriaAI.IRIndexer.Consumers;

public class MentorDeletadoConsumer : IConsumer<MentorDeletadoEvent>
{
    private readonly ElasticsearchService _elastic;
    private readonly ILogger<MentorDeletadoConsumer> _logger;

    public MentorDeletadoConsumer(ElasticsearchService elastic, ILogger<MentorDeletadoConsumer> logger)
    {
        _elastic = elastic;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MentorDeletadoEvent> context)
    {
        var evento = context.Message;
        
        _logger.LogInformation("Recebido MentorDeletadoEvent para o Mentor ID: {MentorId}", evento.Id);

        try
        {
            await _elastic.DeleteAsync(evento.Id.ToString());
            _logger.LogInformation("Mentor deletado com sucesso do índice: {MentorId}", evento.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar o mentor do índice: {MentorId}", evento.Id);
            throw;
        }
    }
}
