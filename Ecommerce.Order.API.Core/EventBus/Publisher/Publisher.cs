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
        #region Properties
        private readonly IConnectionProvider _connectionProvider;
        private readonly string _exchange;
        private readonly string _exchangeType;
        private IModel _channel;
        #endregion

        #region Constructor
        public Publisher(IConnectionProvider connectionProvider, string exchange, string exchangeType)
        {
            _connectionProvider = connectionProvider;
            _exchange = exchange;
            _exchangeType = exchangeType;
            _channel = _connectionProvider.GetConnection().CreateModel();

            CreateConnection();
        }
        #endregion

        #region Create Connection
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
        #endregion

        #region SendMessage
        private void SendMessage(string message, string routeKey)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(_exchange, routeKey, null, body);
        }
        #endregion

        #region PublishNewOrderDetail
        public void PublishNewOrderDetail(OrderDetailModel orderDetail)
        {
            if (orderDetail is null)
                throw new ArgumentException("New Order Detail cannot be null");

            var message = JsonSerializer.Serialize(orderDetail);

            if (_connectionProvider.GetConnection().IsOpen)
                SendMessage(message, "new_order_detail_created");
        }
        #endregion

        #region PublishUpdateOrderDetailUnits
        public void PublishUpdateOrderDetailUnits(OrderDetailUpdateUnitsResponseModel updateOrderDetailUnits)
        {
            if (updateOrderDetailUnits is null)
                throw new ArgumentException("Upda Order Detail Units cannot be null");

            var message = JsonSerializer.Serialize(updateOrderDetailUnits);

            if (_connectionProvider.GetConnection().IsOpen)
                SendMessage(message, "update_order_detail_units");
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (_connectionProvider.GetConnection().IsOpen)
            {
                _channel.Close();
                _connectionProvider.GetConnection().Close();
            }
        } 
        #endregion
    }
}
