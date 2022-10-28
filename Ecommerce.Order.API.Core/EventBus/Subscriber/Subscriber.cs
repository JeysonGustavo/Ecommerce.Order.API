using Ecommerce.Order.API.Core.Context;
using Ecommerce.Order.API.Core.EventBus.Connection;
using Ecommerce.Order.API.Core.EventBus.Publisher;
using Ecommerce.Order.API.Core.Models.Domain;
using Ecommerce.Order.API.Core.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Ecommerce.Order.API.Core.EventBus.Subscriber
{
    public class Subscriber : ISubscriber, IDisposable
    {
        #region Properties
        private readonly IPublisher _publisher;
        private readonly IConnectionProvider _connectionProvider;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly EcommerceDbContext _context;
        private readonly IServiceScope _scope;
        private readonly string _exchange;
        private readonly string _exchangeType;
        private IModel _channel;
        #endregion

        #region Constructor
        public Subscriber(IConnectionProvider connectionProvider, IServiceScopeFactory scopeFactory, string exchange, string exchangeType)
        {
            _scopeFactory = scopeFactory;
            _scope = _scopeFactory.CreateScope();

            _connectionProvider = connectionProvider;
            _publisher = _scope.ServiceProvider.GetRequiredService<IPublisher>();
            _context = _scope.ServiceProvider.GetRequiredService<EcommerceDbContext>();
            _exchange = exchange;
            _exchangeType = exchangeType;
            _channel = _connectionProvider.GetConnection().CreateModel();

            CreateConnection();
        }
        #endregion

        #region InitializeSubscribers
        public void InitializeSubscribers()
        {
            SubscriberProductStockChangedOrderDetailCreated();
            SubscriberProductStockChangedOrderDetailUpdated();
        }
        #endregion

        #region CreateConnection
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

        #region GetMessage
        private string GetMessage(BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();

            var message = Encoding.UTF8.GetString(body);

            return message;
        }
        #endregion

        #region SubscriberProductStockChangedOrderDetailCreated
        private void SubscriberProductStockChangedOrderDetailCreated()
        {
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, _exchange, "product_stock_changed_order_detail_created");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += ProductStockChangedOrderDetailCreatedMessageReceived;

            _channel.BasicConsume(queueName, true, consumer);
        }
        #endregion

        #region ProductStockChangedOrderDetailCreatedMessageReceived
        private void ProductStockChangedOrderDetailCreatedMessageReceived(object? sender, BasicDeliverEventArgs args)
        {
            try
            {
                var message = GetMessage(args);

                var productMessage = JsonSerializer.Deserialize<ProductMessageResponseModel>(message);

                if (productMessage is null)
                    throw new ArgumentException("Could not receive the message from Product service");

                if (productMessage.IsSuccess is false)
                {
                    var orderDetail = _context.OrderDetails.Where(x => x.OrderId == productMessage.OrderId && x.ProductId == productMessage.ProductId).FirstOrDefault();

                    if (orderDetail is null)
                        throw new ArgumentException("Could not receive the message from Product service");

                    _context.OrderDetails.Remove(orderDetail);

                    var product = _context.Products.Where(x => x.Id == productMessage.ProductId).FirstOrDefault();

                    if (product is null)
                        throw new ArgumentException("Could not receive the message from Product service");

                    product.AvailableStock += productMessage.Units;
                    _context.Entry(product).State = EntityState.Modified;

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Exception receiving product stock changed for order created message, Error: {ex.Message}");
            }
        }
        #endregion

        #region SubscriberProductStockChangedOrderDetailUpdated
        private void SubscriberProductStockChangedOrderDetailUpdated()
        {
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, _exchange, "product_stock_changed_order_detail_updated");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += ProductStockChangedOrderDetailUpdatedMessageReceived;

            _channel.BasicConsume(queueName, true, consumer);
        }
        #endregion

        #region ProductStockChangedOrderDetailUpdatedMessageReceived
        private void ProductStockChangedOrderDetailUpdatedMessageReceived(object? sender, BasicDeliverEventArgs e)
        {
            throw new NotImplementedException();
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
