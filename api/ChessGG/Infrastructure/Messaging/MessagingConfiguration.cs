using ChessGG.Application.Interfaces;
using RabbitMQ.Client;

namespace ChessGG.Infrastructure.Messaging;

public static class MessagingConfiguration
{
    public const string AnalisysExchange = "chess.analysis.exchange";
    public const string AnalisysRoutingKey = "analysis";

    public static IServiceCollection ConfigureMessaging(this IServiceCollection services, IConfiguration configs)
    {
        services.AddSingleton(prov => new ConnectionFactory {
            HostName = configs["RabbitMQ_Host"]
                ?? throw new Exception("missing 'RabbitMQ_Host' env."),
            UserName = configs["RabbitMQ_User"]
                ?? throw new Exception("missing 'RabbitMQ_User' env."),
            Password = configs["RabbitMQ_Password"]
                ?? throw new Exception("missing 'RabbitMQ_Password' env.")
        });
        services.AddSingleton<ConnectionManager>();
        services.AddScoped<IPublisher, Publisher>();

        return services;
    }

    public static async Task<WebApplication> UseMessaging(this WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();

        var manager = scope.ServiceProvider.GetRequiredService<ConnectionManager>();
        var connection = await manager.GetAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: AnalisysExchange,
            type: ExchangeType.Direct,
            durable: true
        );

        await channel.QueueDeclareAsync(
            queue: "chess.analysis.queue",
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        await channel.QueueBindAsync(
            queue: "chess.analysis.queue",
            exchange: AnalisysExchange,
            routingKey: AnalisysRoutingKey
        );

        return app;
    }
}