using MentoriaAI.IRIndexer.Services;
using MentoriaAI.IRIndexer.Consumers;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<ElasticsearchService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MentorCriadoConsumer>();
    x.AddConsumer<MentorAtualizadoConsumer>();
    x.AddConsumer<MentorDeletadoConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq";
        var portStr = builder.Configuration["RabbitMQ:Port"];
        ushort port = ushort.TryParse(portStr, out var p) ? p : (ushort)5672;
        var user = builder.Configuration["RabbitMQ:User"] ?? "guest";
        var pass = builder.Configuration["RabbitMQ:Pass"] ?? "guest";
        var queue = builder.Configuration["RabbitMQ:Queue"] ?? "mentores";

        cfg.Host(host, port, "/", h =>
        {
            h.Username(user);
            h.Password(pass);
        });

        cfg.ReceiveEndpoint($"{queue}_indexer", e =>
        {
            e.ConfigureConsumer<MentorCriadoConsumer>(context);
            e.ConfigureConsumer<MentorAtualizadoConsumer>(context);
            e.ConfigureConsumer<MentorDeletadoConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();