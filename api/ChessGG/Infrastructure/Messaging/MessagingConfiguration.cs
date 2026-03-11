using ChessGG.Application.Interfaces;
using RabbitMQ.Client;

namespace ChessGG.Infrastructure.Messaging;

public static class MessagingConfiguration
{
    public const string AnalisysExchange = "chess.analysis.exchange";

    public const string AnalisysQueue = "chess.analysis.queue";
    public const string AnalisysRoutingKey = "analysis";
    
    public const string Retry1Queue = "chess.retry1.queue";
    public const string Retry1RoutingKey = "retry1";
    
    public const string Retry2Queue = "chess.retry2.queue";
    public const string Retry2RoutingKey = "retry2";
    
    
    public const string DLQQueue = "chess.dlq.queue";
    public const string DLQRoutingKey = "dlq";

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

        #region Exchanges

        await channel.ExchangeDeclareAsync(
            exchange: AnalisysExchange,
            type: ExchangeType.Direct,
            durable: true
        );

        #endregion

        #region Queues

        await channel.QueueDeclareAsync(
            queue: AnalisysQueue,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        await channel.QueueDeclareAsync(
            queue: Retry1Queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: new Dictionary<string, object?> {
                { "x-message-ttl", 5_000 },
                { "x-dead-letter-exchange", "chess.ex.analysis" },
                { "x-dead-letter-routing-key", "analysis" }
            }
        );

        await channel.QueueDeclareAsync(
            queue: Retry2Queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: new Dictionary<string, object?> {
                { "x-message-ttl", 30_000 },
                { "x-dead-letter-exchange", "chess.ex.analysis" },
                { "x-dead-letter-routing-key", "analysis" }
            }
        );

        await channel.QueueDeclareAsync(
            queue: DLQQueue,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        #endregion

        #region Binds

        await channel.QueueBindAsync(
            queue: AnalisysQueue,
            exchange: AnalisysExchange,
            routingKey: AnalisysRoutingKey
        );

        await channel.QueueBindAsync(
            queue: Retry1Queue,
            exchange: AnalisysExchange,
            routingKey: Retry1RoutingKey
        );

        await channel.QueueBindAsync(
            queue: Retry2Queue,
            exchange: AnalisysExchange,
            routingKey: Retry2RoutingKey
        );

        await channel.QueueBindAsync(
            queue: DLQQueue,
            exchange: AnalisysExchange,
            routingKey: DLQRoutingKey
        );

        #endregion

        return app;
    }
}