using MentoriaAI.BuscaSemantica.Data;
using MentoriaAI.BuscaSemantica.Repositories;
using MentoriaAI.BuscaSemantica.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EmbeddingsContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseVector()));

builder.Services.AddScoped<IEmbeddingsRepository, EmbeddingsRepository>();
builder.Services.AddSingleton<OpenAIEmbeddingService>();
builder.Services.AddScoped<BuscaSemanticaService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
