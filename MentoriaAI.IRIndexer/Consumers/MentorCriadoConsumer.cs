using MassTransit;
using MentoriaAI.Contracts.Events;
using MentoriaAI.IRIndexer.Models;
using MentoriaAI.IRIndexer.Services;

namespace MentoriaAI.IRIndexer.Consumers;

public class MentorCriadoConsumer : IConsumer<MentorCriadoEvent>
{
    private readonly ElasticsearchService _elastic;
    private readonly ILogger<MentorCriadoConsumer> _logger;

    public MentorCriadoConsumer(ElasticsearchService elastic, ILogger<MentorCriadoConsumer> logger)
    {
        _elastic = elastic;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MentorCriadoEvent> context)
    {
        var evento = context.Message;
        
        _logger.LogInformation("Recebido MentorCriadoEvent para o Mentor ID: {MentorId}", evento.Id);

        var doc = new MentorDocument
        {
            Id = evento.Id,
            Nome = evento.Nome,
            Area = evento.Area,
            Tecnologias = evento.Tecnologias,
            Descricao = evento.Descricao,
            DataAtualizacao = DateTime.UtcNow
        };

        try
        {
            await _elastic.IndexAsync(doc);
            _logger.LogInformation("Mentor indexado com sucesso: {MentorId}", doc.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao indexar o mentor criado: {MentorId}", doc.Id);
            throw; // Repassa a exceção para que o MassTransit possa tentar novamente (retry)
        }
    }
}
