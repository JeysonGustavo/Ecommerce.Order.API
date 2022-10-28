using Ecommerce.Order.API.Core.EventBus.Connection;
using Ecommerce.Order.API.Core.Models.Domain;
using Ecommerce.Order.API.Core.Models.Response;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Ecommerce.Order.API.Core.EventBus.Publisher
{
    public class Publisher : IPublisher, IDisposable
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly string _exchange;
        private readonly string _exchangeType;
        private IModel _channel;

        public Publisher(IConnectionProvider connectionProvider, string exchange, string exchangeType)
        {
            _connectionProvider = connectionProvider;
            _exchange = exchange;
            _exchangeType = exchangeType;
            _channel = _connectionProvider.GetConnection().CreateModel();

            CreateConnection();
        }

        private void CreateConnection()
        {
            try
            {
                _channel.ExchangeDeclare(exchange: _exchange, type: _exchangeType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> RabbitMQ connection error: {ex.Message}");
                throw new Exception("RabbitMQ Connection error");
            }
        }

        private void SendMessage(string message, string routeKey)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(_exchange, routeKey, null, body);
        }

        public void PublishNewOrderDetail(OrderDetailModel orderDetail)
        {
            if (orderDetail is null)
                throw new ArgumentException("New Order Detail cannot be null");

            var message = JsonSerializer.Serialize(orderDetail);

            if (_connectionProvider.GetConnection().IsOpen)
                SendMessage(message, "new_order_detail_created");
        }

        public void PublishUpdateOrderDetailUnits(OrderDetailUpdateUnitsResponseModel updateOrderDetailUnits)
        {
            if (updateOrderDetailUnits is null)
                throw new ArgumentException("Upda Order Detail Units cannot be null");

            var message = JsonSerializer.Serialize(updateOrderDetailUnits);

            if (_connectionProvider.GetConnection().IsOpen)
                SendMessage(message, "update_order_detail_units");
        }

        public void Dispose()
        {
            if (_connectionProvider.GetConnection().IsOpen)
            {
                _channel.Close();
                _connectionProvider.GetConnection().Close();
            }
        }
    }
}
