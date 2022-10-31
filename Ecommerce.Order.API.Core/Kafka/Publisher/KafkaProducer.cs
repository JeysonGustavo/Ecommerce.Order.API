using Confluent.Kafka;
using Ecommerce.Order.API.Core.Kafka.Connection;
using Ecommerce.Order.API.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

            await _kafkaConnectionProvider.GetProducer().ProduceAsync("kafka_product_stock_changed_order_detail_created", new Message<Null, string> { Value = message });

            _kafkaConnectionProvider.GetProducer().Flush();
        } 
        #endregion
    }
}
