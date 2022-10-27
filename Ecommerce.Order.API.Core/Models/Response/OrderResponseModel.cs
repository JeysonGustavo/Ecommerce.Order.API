namespace Ecommerce.Order.API.Core.Models.Response
{
    public class OrderResponseModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Units { get; set; }
        public ProductResponseModel Product { get; set; } = new ProductResponseModel();
    }
}
