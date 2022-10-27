namespace Ecommerce.Order.API.Core.Models.Response
{
    public class OrderDetailResponseModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Units { get; set; }
        public ProductResponseModel Product { get; set; }
    }
}
