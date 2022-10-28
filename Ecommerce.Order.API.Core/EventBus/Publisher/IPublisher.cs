using Ecommerce.Order.API.Core.Models.Domain;
using Ecommerce.Order.API.Core.Models.Response;

namespace Ecommerce.Order.API.Core.EventBus.Publisher
{
    public interface IPublisher
    {
        void PublishNewOrderDetail(OrderDetailModel orderDetail);

        void PublishUpdateOrderDetailUnits(OrderDetailUpdateUnitsResponseModel updateOrderDetailUnits);
    }
}
