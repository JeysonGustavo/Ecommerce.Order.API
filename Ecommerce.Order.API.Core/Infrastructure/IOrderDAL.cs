using Ecommerce.Order.API.Core.Models.Domain;

namespace Ecommerce.Order.API.Core.Infrastructure
{
    public interface IOrderDAL
    {
        Task<IEnumerable<OrderModel>> GetAllOrders();

        Task<OrderModel?> GetOrderById(int id);

        Task<bool> OrderExists(int id);

        Task CreateOrder(OrderModel order);
    }
}
