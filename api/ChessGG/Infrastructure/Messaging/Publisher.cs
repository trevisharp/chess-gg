using System.Text;
using System.Text.Json;
using ChessGG.Application.Interfaces;
using RabbitMQ.Client;

namespace ChessGG.Infrastructure.Messaging;

public class Publisher(ConnectionManager manager) : IAsyncDisposable, IPublisher
{
    IChannel? channel = null;

    public async Task Publish<T>(string exchange, string routingKey, T message)
    {
        var connection = await manager.GetAsync();
        channel ??= await connection.CreateChannelAsync();

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);
        var props = new BasicProperties {
            Persistent = true,
            MessageId = Guid.NewGuid().ToString()
        };

        await channel.BasicPublishAsync(
            exchange: exchange, 
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props, 
            body: body
        );
    }

    public async ValueTask DisposeAsync()
    {
        if (channel is null)
            return;
        
        await channel.CloseAsync();
        await channel.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}