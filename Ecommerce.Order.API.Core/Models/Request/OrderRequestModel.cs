namespace Ecommerce.Order.API.Core.Models.Request
{
    public class OrderRequestModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Units { get; set; }
    }
}
