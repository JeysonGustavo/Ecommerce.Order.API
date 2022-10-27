using AutoMapper;
using Ecommerce.Order.API.Core.Manager;
using Ecommerce.Order.API.Core.Models.Domain;
using Ecommerce.Order.API.Core.Models.Request;
using Ecommerce.Order.API.Core.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Order.API.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        private readonly IMapper _mapper;

        public OrderController(IOrderManager orderManager, IMapper mapper)
        {
            _orderManager = orderManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllOrders()
        {
            var orders = await _orderManager.GetAllOrders();

            return Ok(_mapper.Map<IEnumerable<OrderResponseModel>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderById(int id)
        {
            var order = await _orderManager.GetOrderById(id);

            if (order is null)
                return NotFound();

            return Ok(_mapper.Map<OrderResponseModel>(order));
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(OrderRequestModel requestModel)
        {
            var order = _mapper.Map<OrderModel>(requestModel);

            await _orderManager.CreateOrder(order);

            var response = _mapper.Map<OrderResponseModel>(order);

            return CreatedAtAction("GetOrderById", new { Id = response.Id }, response);
        }
    }
}
