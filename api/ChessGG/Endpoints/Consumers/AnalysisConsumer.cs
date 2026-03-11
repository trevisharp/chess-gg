using System.Text;
using System.Text.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChessGG.Endpoints.Consumers;

using Application.UseCases.GenerateAnalisys;
using Application.Interfaces;
using Infrastructure.Messaging;
using System.Runtime.Intrinsics.Arm;

public class AnalysisConsumer(
    ConnectionManager manager,
    GenerateAnalisysUseCase useCase,
    IPublisher publisher,
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
                var json = JsonSerializer.Deserialize<GenerateAnalisysRequest>(message) 
                    ?? throw new Exception("Invalid message format.");
                await useCase.RunAsync(json);
            }
            catch (Exception ex)
            {
                if (logger.IsEnabled(LogLevel.Error))
                    logger.LogError("Error on analisys processing {message}. Error: {ex}", message, ex);

                int deaths = 0;
                var headers = ea.BasicProperties.Headers;
                if (headers != null && headers.TryGetValue("x-death", out var value))
                {
                    var deathList = (List<object>)value!;
                    var deathEntry = (Dictionary<string, object>)deathList[0];
                    deaths = Convert.ToInt32(deathEntry["count"]);
                }

                var props = new BasicProperties {
                    Headers = ea.BasicProperties.Headers,
                    CorrelationId = ea.BasicProperties.CorrelationId,
                    MessageId = ea.BasicProperties.MessageId,
                    ContentType = ea.BasicProperties.ContentType,
                    ContentEncoding = ea.BasicProperties.ContentEncoding,
                    DeliveryMode = ea.BasicProperties.DeliveryMode
                };

                if (deaths == 0)
                    await publisher.Publish("chess.analysis.exchange", "retry1", message, props);
                else if (deaths == 1)
                    await publisher.Publish("chess.analysis.exchange", "retry2", message, props);
                else
                    await publisher.Publish("chess.analysis.exchange", "dlq", message, props);
            }
            finally
            {
                await channel.BasicAckAsync(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false
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
        if (channel is not null)
            await channel.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}