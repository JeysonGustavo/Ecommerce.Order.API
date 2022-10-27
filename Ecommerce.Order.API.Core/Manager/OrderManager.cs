using Ecommerce.Order.API.Core.Infrastructure;
using Ecommerce.Order.API.Core.Models.Domain;
using Ecommerce.Order.API.Core.Models.Request;

namespace Ecommerce.Order.API.Core.Manager
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderDAL _orderDAL;

        public OrderManager(IOrderDAL orderDAL)
        {
            _orderDAL = orderDAL;
        }

        public async Task CreateOrder(OrderModel order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            order.Acitve = true;

            await _orderDAL.CreateOrder(order);
        }

        public async Task<bool> InactiveOrder(int id)
        {
            if (id < 1)
                throw new ArgumentException("Id field is required");

            var order = await _orderDAL.GetOrderById(id);
            if (order is null)
                throw new ArgumentException("Order not found");

            return await _orderDAL.InactiveOrder(order);
        }

        public async Task<OrderModel?> GetOrderById(int id)
        {
            var order = await _orderDAL.GetOrderById(id);

            return order ?? new OrderModel();
        }
       
        public async Task<bool> CreateOrderDetail(OrderDetailModel orderDetail)
        {
            if (orderDetail is null)
                throw new ArgumentNullException(nameof(orderDetail));

            var order = await _orderDAL.GetOrderById(orderDetail.OrderId);
            if (order is null)
                throw new ArgumentException("Order not found");

            if (order.Acitve is false)
                throw new ArgumentException("Order is inactived");

            return await _orderDAL.CreateOrderDetail(orderDetail);
        }

        public async Task<bool> UpdateOrderDetailUnits(OrderDetailUpdateUnitsRequestModel requestModel)
        {
            if (requestModel.OrderId < 1)
                throw new ArgumentException("Id field is required");

            if (requestModel.Units < 1)
                throw new ArgumentException("Unit field is required");

            var orderDetail = await _orderDAL.GetOrderDetailById(requestModel.OrderId);
            if (orderDetail is null)
                throw new ArgumentException("Order Detail not found");

            orderDetail.Units = requestModel.Units;

            return await _orderDAL.UpdateOrderDetail(orderDetail);
        }

        public async Task<bool> DeleteOrderDetail(int id)
        {
            if (id < 1)
                throw new ArgumentException("Id field is required");

            var orderDetail = await _orderDAL.GetOrderDetailById(id);
            if (orderDetail is null)
                throw new ArgumentException("Order Detail not found");

            return await _orderDAL.DeleteOrderDetail(orderDetail);
        }
    }
}
