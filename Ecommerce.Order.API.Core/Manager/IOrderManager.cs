using Ecommerce.Order.API.Core.Models.Domain;

namespace Ecommerce.Order.API.Core.Manager
{
    public interface IOrderManager
    {
        Task<IEnumerable<OrderModel>> GetAllOrders();

        Task<OrderModel?> GetOrderById(int id);

        Task CreateOrder(OrderModel order);
    }
}
