namespace Ecommerce.Order.API.Core.Models.Request
{
    public class OrderDetailUpdateUnitsRequestModel
    {
        public int OrderId { get; set; }
        public int Units { get; set; }
    }
}
