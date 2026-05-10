using Elastic.Clients.Elasticsearch;
using MentoriaAI.IRIndexer.Models;

namespace MentoriaAI.IRIndexer.Services;

public class ElasticsearchService
{
    private readonly ElasticsearchClient _client;

    public ElasticsearchService(IConfiguration config)
    {
        var url = config["Elastic:Url"] ?? "http://localhost:9200";
        var settings = new ElasticsearchClientSettings(
            new Uri(url))
            .DefaultIndex("mentores");

        _client = new ElasticsearchClient(settings);
    }

    public async Task IndexAsync(MentorDocument doc)
    {
        var response = await _client.IndexAsync(doc, i => i
            .Id(doc.Id.ToString())
            .Refresh(Refresh.WaitFor));

        if (!response.IsValidResponse)
        {
            throw new Exception(response.DebugInformation);
        }
    }

    public async Task DeleteAsync(string id)
    {
        await _client.DeleteAsync<MentorDocument>(id);
    }
}