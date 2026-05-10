using MentoriaAI.BuscaSemantica.Data;
using MentoriaAI.BuscaSemantica.Repositories;
using MentoriaAI.BuscaSemantica.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EmbeddingsContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseVector()));

// Repositories
builder.Services.AddScoped<IEmbeddingsRepository, EmbeddingsRepository>();

// Services
builder.Services.AddScoped<BuscaSemanticaService>();
builder.Services.AddSingleton<OpenAIChatService>();
builder.Services.AddSingleton<OpenAIEmbeddingService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

try
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Console.Error.WriteLine("Fatal error starting application:");
    Console.Error.WriteLine(ex.ToString());
    Environment.ExitCode = -1;
}