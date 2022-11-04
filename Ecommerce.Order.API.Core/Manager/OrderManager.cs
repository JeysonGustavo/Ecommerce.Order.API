using AutoMapper;
using Ecommerce.Order.API.Core.EventBus.Publisher;
using Ecommerce.Order.API.Core.Infrastructure;
using Ecommerce.Order.API.Core.Kafka.Publisher;
using Ecommerce.Order.API.Core.Models.Domain;
using Ecommerce.Order.API.Core.Models.Request;
using Ecommerce.Order.API.Core.Models.Response;

namespace Ecommerce.Order.API.Core.Manager
{
    public class OrderManager : IOrderManager
    {
        #region Properties
        private readonly IOrderDAL _orderDAL;
        //private readonly IPublisher _publisher;
        private readonly IMapper _mapper;
        private readonly IKafkaProducer _kafkaProducer;
        #endregion

        #region Constructor
        public OrderManager(IOrderDAL orderDAL, IMapper mapper, IKafkaProducer kafkaProducer)
        {
            _orderDAL = orderDAL;
            //_publisher = publisher;
            _mapper = mapper;
            _kafkaProducer = kafkaProducer;
        }
        #endregion

        #region CreateOrder
        public async Task CreateOrder(OrderModel order)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            order.Acitve = true;

            await _orderDAL.CreateOrder(order);
        }
        #endregion

        #region InactiveOrder
        public async Task<bool> InactiveOrder(int id)
        {
            if (id < 1)
                throw new ArgumentException("Id field is required");

            var order = await _orderDAL.GetOrderById(id);
            if (order is null)
                throw new ArgumentException("Order not found");

            return await _orderDAL.InactiveOrder(order);
        }
        #endregion

        #region GetOrderById
        public async Task<OrderModel?> GetOrderById(int id)
        {
            var order = await _orderDAL.GetOrderById(id);

            return order ?? new OrderModel();
        }
        #endregion

        #region CreateOrderDetail
        public async Task<bool> CreateOrderDetail(OrderDetailModel orderDetail)
        {
            if (orderDetail is null)
                throw new ArgumentNullException(nameof(orderDetail));

            var order = await _orderDAL.GetOrderById(orderDetail.OrderId);
            if (order is null)
                throw new ArgumentException("Order not found");

            if (order.Acitve is false)
                throw new ArgumentException("Order is inactived");

            if (await _orderDAL.IsProductExistsForTheOrderDetail(orderDetail.OrderId, orderDetail.ProductId) is true)
                throw new ArgumentException("The Product is already on this order, change the Units");

            
            bool response = await _orderDAL.CreateOrderDetail(orderDetail);

            if (response is true)
            {
                // RabbitMQ
                //_publisher.PublishNewOrderDetail(orderDetail);

                // Kafka
                await _kafkaProducer.PublishNewOrderDetail(orderDetail);
            }

            return response;
        }
        #endregion

        #region UpdateOrderDetailUnits
        public async Task<bool> UpdateOrderDetailUnits(OrderDetailUpdateUnitsRequestModel requestModel)
        {
            if (requestModel.OrderDetailId < 1)
                throw new ArgumentException("Id field is required");

            if (requestModel.Units < 1)
                throw new ArgumentException("Unit field is required");

            var orderDetail = await _orderDAL.GetOrderDetailById(requestModel.OrderDetailId);
            if (orderDetail is null)
                throw new ArgumentException("Order Detail not found");

            var updateOrderDetail = _mapper.Map<OrderDetailUpdateUnitsResponseModel>(orderDetail);
            updateOrderDetail.OldUnits = orderDetail.Units;
            updateOrderDetail.NewUnits = requestModel.Units;

            orderDetail.Units = requestModel.Units;

            bool response = await _orderDAL.UpdateOrderDetail(orderDetail);

            if (response is true)
            {
                // RabbitMQ
                //_publisher.PublishUpdateOrderDetailUnits(updateOrderDetail);

                // Kafka
                await _kafkaProducer.PublishUpdateOrderDetailUnits(updateOrderDetail);
            }

            return response;
        }
        #endregion

        #region DeleteOrderDetail
        public async Task<bool> DeleteOrderDetail(int id)
        {
            if (id < 1)
                throw new ArgumentException("Id field is required");

            var orderDetail = await _orderDAL.GetOrderDetailById(id);
            if (orderDetail is null)
                throw new ArgumentException("Order Detail not found");

            bool response = await _orderDAL.DeleteOrderDetail(orderDetail);

            if (response is true)
            {
                // RabbitMQ
                //_publisher.PublishDeletedOrderDetail(orderDetail);

                // Kafka
                await _kafkaProducer.PublishDeletedOrderDetail(orderDetail);
            }                

            return response;
        } 
        #endregion
    }
}
