using Confluent.Kafka;
using Ecommerce.Order.API.Core.Kafka.Connection;
using Ecommerce.Order.API.Core.Models.Domain;
using Ecommerce.Order.API.Core.Models.Response;
using System.Text.Json;

namespace Ecommerce.Order.API.Core.Kafka.Publisher
{
    public class KafkaProducer : IKafkaProducer
    {
        #region Properties
        private readonly IKafkaConnectionProvider _kafkaConnectionProvider;
        #endregion

        #region Constructor
        public KafkaProducer(IKafkaConnectionProvider kafkaConnectionProvider)
        {
            _kafkaConnectionProvider = kafkaConnectionProvider;
        }
        #endregion

        #region PublishNewOrderDetail
        public async Task PublishNewOrderDetail(OrderDetailModel orderDetail)
        {
            if (orderDetail is null)
                throw new ArgumentException("New Order Detail cannot be null");

            var message = JsonSerializer.Serialize(orderDetail);

            await _kafkaConnectionProvider.GetProducer().ProduceAsync("kafka_new_order_detail_created", new Message<Null, string> { Value = message });

            _kafkaConnectionProvider.GetProducer().Flush();
        }
        #endregion

        #region PublishUpdateOrderDetailUnits
        public async Task PublishUpdateOrderDetailUnits(OrderDetailUpdateUnitsResponseModel updateOrderDetailUnits)
        {
            if (updateOrderDetailUnits is null)
                throw new ArgumentException("Update Order Detail Units cannot be null");

            var message = JsonSerializer.Serialize(updateOrderDetailUnits);

            await _kafkaConnectionProvider.GetProducer().ProduceAsync("kafka_updated_order_detail_units", new Message<Null, string> { Value = message });

            _kafkaConnectionProvider.GetProducer().Flush();
        }
        #endregion

        #region PublishDeletedOrderDetail
        public async Task PublishDeletedOrderDetail(OrderDetailModel orderDetail)
        {
            if (orderDetail is null)
                throw new ArgumentException("Order Detail cannot be null");

            var message = JsonSerializer.Serialize(orderDetail);

            await _kafkaConnectionProvider.GetProducer().ProduceAsync("kafka_order_detail_deleted", new Message<Null, string> { Value = message });

            _kafkaConnectionProvider.GetProducer().Flush();
        }
        #endregion
    }
}
