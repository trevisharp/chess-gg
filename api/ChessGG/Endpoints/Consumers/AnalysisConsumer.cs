using System.Text;
using System.Text.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChessGG.Endpoints.Consumers;

using Application.UseCases.GenerateAnalisys;
using Infrastructure.Messaging;

public class AnalysisConsumer(
    ConnectionManager manager,
    GenerateAnalisysUseCase useCase,
    ILogger<AnalysisConsumer> logger) : BackgroundService
{
    IChannel? channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = await manager.GetAsync();
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
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var json = JsonSerializer
                    .Deserialize<GenerateAnalisysRequest>(message);
                if (json is null)
                    throw new Exception("vishhh");
                
                await useCase.RunAsync(json);

                await channel.BasicAckAsync(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false
                );
            }
            catch (Exception ex)
            {
                if (logger.IsEnabled(LogLevel.Error))
                    logger.LogError("Error on analisys processing {}. Error: {}", message, ex);
                
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