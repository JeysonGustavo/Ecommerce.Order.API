using Ecommerce.Order.API.Core.Infrastructure;
using Ecommerce.Order.API.Core.Models.Domain;

namespace Ecommerce.Order.API.Core.Manager
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderDAL _orderDAL;

        public OrderManager(IOrderDAL orderDAL)
        {
            _orderDAL = orderDAL;
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrders()
        {
            var orders = await _orderDAL.GetAllOrders();

            return orders ?? new List<OrderModel>();
        }

        public async Task<OrderModel?> GetOrderById(int id)
        {
            var order = await _orderDAL.GetOrderById(id);

            return order ?? new OrderModel();
        }

        public async Task CreateOrder(OrderModel order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            await _orderDAL.CreateOrder(order);
        }
    }
}
