using Ecommerce.Order.API.Core.Models.Domain;
using Ecommerce.Order.API.Core.Models.Request;

namespace Ecommerce.Order.API.Core.Manager
{
    public interface IOrderManager
    {
        Task CreateOrder(OrderModel order);

        Task<bool> InactiveOrder(int id);

        Task<OrderModel?> GetOrderById(int id);

        Task<bool> CreateOrderDetail(OrderDetailModel orderDetail);

        Task<bool> UpdateOrderDetailUnits(OrderDetailUpdateUnitsRequestModel requestModel);

        Task<bool> DeleteOrderDetail(int id);
    }
}
