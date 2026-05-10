using MassTransit;
using MentoriaAI.Contracts.Events;
using MentoriaAI.IRIndexer.Models;
using MentoriaAI.IRIndexer.Services;

namespace MentoriaAI.IRIndexer.Consumers;

public class MentorAtualizadoConsumer : IConsumer<MentorAtualizadoEvent>
{
    private readonly ElasticsearchService _elastic;
    private readonly ILogger<MentorAtualizadoConsumer> _logger;

    public MentorAtualizadoConsumer(ElasticsearchService elastic, ILogger<MentorAtualizadoConsumer> logger)
    {
        _elastic = elastic;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MentorAtualizadoEvent> context)
    {
        var evento = context.Message;
        
        _logger.LogInformation("Recebido MentorAtualizadoEvent para o Mentor ID: {MentorId}", evento.Id);

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
            _logger.LogInformation("Mentor atualizado/indexado com sucesso: {MentorId}", doc.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar/indexar o mentor: {MentorId}", doc.Id);
            throw;
        }
    }
}
