using Ecommerce.Order.API.Core.Context;
using Ecommerce.Order.API.Core.Kafka.Connection;
using Ecommerce.Order.API.Core.Models.Domain;
using Ecommerce.Order.API.Core.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace Ecommerce.Order.API.Core.Kafka.Consumer
{
    public class KafkaConsumer : IKafkaConsumer
    {
        #region Properties
        private readonly IKafkaConnectionProvider _kafkaConnectionProvider;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceScope _scope;
        private readonly EcommerceDbContext _context;
        public CancellationTokenSource _cancellationToken = new();
        #endregion

        #region Constructor
        public KafkaConsumer(IKafkaConnectionProvider kafkaConnectionProvider, IServiceScopeFactory scopeFactory)
        {
            _kafkaConnectionProvider = kafkaConnectionProvider;

            _scopeFactory = scopeFactory;
            _scope = _scopeFactory.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<EcommerceDbContext>();
        }
        #endregion

        #region InitializeConsumers
        public void InitializeConsumers()
        {
            List<string> topics = new List<string>();
            topics.Add("kafka_product_stock_changed_order_detail_created");
            topics.Add("kafka_product_stock_changed_order_detail_updated");
            topics.Add("kafka_product_stock_changed_order_detail_deleted");

            SubscribeTopics(topics);
        }
        #endregion

        private async void SubscribeTopics(List<string> topics)
        {
            await Task.Run(() =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        _kafkaConnectionProvider.GetConsumer().Subscribe(topics);

                        var response = _kafkaConnectionProvider.GetConsumer().Consume(_cancellationToken.Token);

                        if (response is not null)
                        {
                            switch (response.Topic)
                            {
                                case "kafka_product_stock_changed_order_detail_created":
                                    ProductStockChangedOrderDetailCreatedMessageReceived(response.Message.Value);
                                    break;

                                case "kafka_product_stock_changed_order_detail_updated":
                                    ProductStockChangedOrderDetailUpdatedMessageReceived(response.Message.Value);
                                    break;

                                case "kafka_product_stock_changed_order_detail_deleted":
                                    ProductStockChangedOrderDetailDeletedMessageReceived(response.Message.Value);
                                    break;

                                default:
                                    Console.WriteLine($"--> Missing Topic, name: {response.Topic}");
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--> Exception receiving product message, Error: {ex.Message}");
                    }
                }
            });
        }

        #region ProductStockChangedOrderDetailCreatedMessageReceived
        private async void ProductStockChangedOrderDetailCreatedMessageReceived(string message)
        {
            try
            {
                var productMessage = JsonSerializer.Deserialize<ProductMessageResponseModel>(message);

                if (productMessage is null)
                    throw new ArgumentException("Could not receive the message from Product service");

                if (productMessage.IsSuccess is false)
                {
                    var orderDetail = await _context.OrderDetails.Where(x => x.OrderId == productMessage.OrderId && x.ProductId == productMessage.ProductId).FirstOrDefaultAsync();

                    if (orderDetail is null)
                        throw new ArgumentException("Could not receive the message from Product service");

                    _context.OrderDetails.Remove(orderDetail);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Exception receiving product stock changed for order created message, Error: {ex.Message}");
            }
        }
        #endregion

        #region ProductStockChangedOrderDetailUpdatedMessageReceived
        private async void ProductStockChangedOrderDetailUpdatedMessageReceived(string message)
        {
            try
            {
                var productMessage = JsonSerializer.Deserialize<ProductMessageResponseModel>(message);

                if (productMessage is null)
                    throw new ArgumentException("Could not receive the message from Product service");

                if (productMessage.IsSuccess is false)
                {
                    var orderDetail = await _context.OrderDetails.Where(x => x.OrderId == productMessage.OrderId && x.ProductId == productMessage.ProductId).FirstOrDefaultAsync();

                    if (orderDetail is null)
                        throw new ArgumentException("Could not receive the message from Product service");

                    orderDetail.Units = productMessage.Units;
                    _context.Entry(orderDetail).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Exception receiving product stock changed for order updated message, Error: {ex.Message}");
            }
        }
        #endregion

        #region ProductStockChangedOrderDetailDeletedMessageReceived
        private async void ProductStockChangedOrderDetailDeletedMessageReceived(string message)
        {
            try
            {
                var productMessage = JsonSerializer.Deserialize<ProductMessageResponseModel>(message);

                if (productMessage is null)
                    throw new ArgumentException("Could not receive the message from Product service");

                if (productMessage.IsSuccess is false)
                {
                    var orderDetail = new OrderDetailModel { OrderId = productMessage.OrderId, ProductId = productMessage.ProductId, Units = productMessage.Units };
                    await _context.OrderDetails.AddAsync(orderDetail);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Exception receiving product stock changed for order deleted message, Error: {ex.Message}");
            }
        }
        #endregion
    }
}
