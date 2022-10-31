using Ecommerce.Order.API.Core.Models.Domain;

namespace Ecommerce.Order.API.Core.Kafka.Publisher
{
    public interface IKafkaProducer
    {
        Task PublishNewOrderDetail(OrderDetailModel orderDetail);
    }
}
