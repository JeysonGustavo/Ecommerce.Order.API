using Ecommerce.Order.API.Core.Models.Domain;

namespace Ecommerce.Order.API.Core.Models.Response
{
    public class OrderResponseModel
    {
        public int Id { get; set; }
        public List<OrderDetailResponseModel> OrderDetails { get; set; } = new List<OrderDetailResponseModel>();
    }
}
