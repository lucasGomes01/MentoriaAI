using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o YARP e o proxy reverso
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Redireciona as rotas
app.MapReverseProxy();

app.Run();
