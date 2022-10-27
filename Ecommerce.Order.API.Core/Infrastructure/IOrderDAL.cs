using Ecommerce.Order.API.Core.Models.Domain;

namespace Ecommerce.Order.API.Core.Infrastructure
{
    public interface IOrderDAL
    {
        Task CreateOrder(OrderModel order);

        Task<bool> InactiveOrder(OrderModel order);

        Task<OrderModel?> GetOrderById(int id);

        Task<bool> OrderExists(int id);

        Task<bool> CreateOrderDetail(OrderDetailModel orderDetail);

        Task<bool> UpdateOrderDetail(OrderDetailModel orderDetail);

        Task<bool> DeleteOrderDetail(OrderDetailModel orderDetail);

        Task<OrderDetailModel?> GetOrderDetailById(int id);
    }
}
