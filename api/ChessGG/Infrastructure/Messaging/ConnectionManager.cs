using RabbitMQ.Client;

namespace ChessGG.Infrastructure.Messaging;

public class ConnectionManager(ConnectionFactory factory)
{
    IConnection? connection = null;

    public async Task<IConnection> GetAsync()
    {
        if (connection != null && connection.IsOpen)
            return connection;

        var delay = TimeSpan.FromSeconds(1);

        for (int i = 0; i < 4; i++)
        {
            try
            {
                connection = await factory.CreateConnectionAsync();
                return connection;
            }
            catch
            {
                await Task.Delay(delay);
                delay *= 2;
            }
        }
        
        throw new Exception("RabbitMQ is down");
    }
}