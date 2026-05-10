using Elastic.Clients.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var url = builder.Configuration["Elastic:Url"] ?? "http://localhost:9200";
var settings = new ElasticsearchClientSettings(new Uri(url)).DefaultIndex("mentores");
builder.Services.AddSingleton(new ElasticsearchClient(settings));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
