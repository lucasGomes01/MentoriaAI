using Elastic.Clients.Elasticsearch;
using MentoriaAI.IRQueryService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MentoriaAI.IRQueryService.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly ElasticsearchClient _client;
    private readonly ILogger<SearchController> _logger;

    public SearchController(ElasticsearchClient client, ILogger<SearchController> logger)
    {
        _client = client;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return BadRequest("O termo de busca 'q' é obrigatório.");
        }

        _logger.LogInformation("Realizando busca IR para o termo: {Query}", q);

        var response = await _client.SearchAsync<MentorDto>(s => s
            .Query(query => query
                .MultiMatch(m => m
                    .Fields(new[] { "nome", "area", "tecnologias", "descricao" })
                    .Query(q)
                )
            )
            .Size(20) // Retornar os top 20 resultados
        );

        if (!response.IsValidResponse)
        {
            _logger.LogError("Erro na consulta Elasticsearch: {Error}", response.DebugInformation);
            return StatusCode(500, "Erro ao consultar o serviço de busca.");
        }
        
        _logger.LogInformation("Busca IR realizada com sucesso para o termo: {Query} : {@Resultados}", q, response.Documents);

        return Ok(response.Documents);
    }
}
