using RabbitMQ.Client;

namespace Ecommerce.Order.API.Core.EventBus.Connection
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}
