namespace Ecommerce.Order.API.Core.Models.Request
{
    public class OrderDetailRequestModel
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Units { get; set; }
    }
}
