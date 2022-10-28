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
        #region Properties
        private readonly IOrderManager _orderManager;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public OrderController(IOrderManager orderManager, IMapper mapper)
        {
            _orderManager = orderManager;
            _mapper = mapper;
        }
        #endregion

        #region CreateOrder
        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderRequestModel requestModel)
        {
            var order = _mapper.Map<OrderModel>(requestModel);

            await _orderManager.CreateOrder(order);

            var response = _mapper.Map<OrderResponseModel>(order);

            return CreatedAtAction("GetOrderById", new { Id = response.Id }, response);
        }
        #endregion

        #region GetOrderById
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderById(int id)
        {
            var order = await _orderManager.GetOrderById(id);

            if (order is null)
                return NotFound();

            return Ok(_mapper.Map<OrderResponseModel>(order));
        }
        #endregion

        #region DeleteOrder
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var response = await _orderManager.InactiveOrder(id);

                return Ok(response);
            }
            catch (ArgumentException ae)
            {
                return Ok(ae.Message);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        #endregion

        #region CreateOrderDetail
        [HttpPost]
        [Route("CreateOrderDetail")]
        public async Task<ActionResult> CreateOrderDetail(OrderDetailRequestModel requestModel)
        {
            try
            {
                var orderDetail = _mapper.Map<OrderDetailModel>(requestModel);

                var response = await _orderManager.CreateOrderDetail(orderDetail);

                return Ok(response);
            }
            catch (ArgumentException ae)
            {
                return Ok(ae.Message);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        #endregion

        #region UpdateOrderDetailUnits
        [HttpPut]
        [Route("UpdateOrderDetailUnits")]
        public async Task<IActionResult> UpdateOrderDetailUnits(OrderDetailUpdateUnitsRequestModel requestModel)
        {
            try
            {
                bool response = await _orderManager.UpdateOrderDetailUnits(requestModel);

                return Ok(response);
            }
            catch (ArgumentException ae)
            {
                return Ok(ae.Message);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        #endregion

        #region DeleteOrderDetail
        [HttpDelete]
        [Route("DeleteOrderDetail/{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            try
            {
                var response = await _orderManager.DeleteOrderDetail(id);

                return Ok(response);
            }
            catch (ArgumentException ae)
            {
                return Ok(ae.Message);
            }
            catch (Exception)
            {
                return NotFound();
            }
        } 
        #endregion
    }
}
