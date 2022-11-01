using Ecommerce.Order.API.Core.Models.Domain;
using Ecommerce.Order.API.Core.Models.Response;

namespace Ecommerce.Order.API.Core.Kafka.Publisher
{
    public interface IKafkaProducer
    {
        Task PublishNewOrderDetail(OrderDetailModel orderDetail);

        Task PublishUpdateOrderDetailUnits(OrderDetailUpdateUnitsResponseModel updateOrderDetailUnits);

        Task PublishDeletedOrderDetail(OrderDetailModel orderDetail);
    }
}
