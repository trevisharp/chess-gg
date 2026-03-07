namespace ChessGG.Application.Interfaces;

public interface IPublisher
{
    Task Publish<T>(string exchange, string routingKey, T message);
}