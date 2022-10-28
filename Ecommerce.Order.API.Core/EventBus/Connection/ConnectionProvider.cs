using RabbitMQ.Client;

namespace Ecommerce.Order.API.Core.EventBus.Connection
{
    public class ConnectionProvider : IConnectionProvider, IDisposable
    {
        private readonly ConnectionFactory _factory;

        private readonly IConnection _connection;

        public ConnectionProvider(string url)
        {
            _factory = new ConnectionFactory
            {
                Uri = new Uri(url)
            };

            _connection = _factory.CreateConnection();
        }

        public ConnectionProvider(string hostName, int port, string userName = "guest", string password = "guest")
        {
            _factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
            };

            _connection = _factory.CreateConnection();
        }

        public IConnection GetConnection() => _connection;

        public void Dispose()
        {
            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}
