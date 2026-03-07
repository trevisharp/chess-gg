using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChessGG.Endpoints.Consumers;

public class AnalysisConsumer(IConnection connection) : BackgroundService
{
    IChannel? channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: "chess.analysis.queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken
        );

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                Console.WriteLine($"[Consumer] Received: {message}");

                await channel.BasicAckAsync(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex}");
                
                await channel.BasicNackAsync(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false,
                    requeue: true
                );
            }
        };

        await channel.BasicQosAsync(0, 1, false, stoppingToken);
        await channel.BasicConsumeAsync(
            queue: "chess.analysis.queue",
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken
        );
        
        await Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        channel?.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}