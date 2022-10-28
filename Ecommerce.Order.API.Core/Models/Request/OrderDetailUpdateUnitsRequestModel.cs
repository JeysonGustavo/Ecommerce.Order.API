namespace Ecommerce.Order.API.Core.Models.Request
{
    public class OrderDetailUpdateUnitsRequestModel
    {
        public int OrderDetailId { get; set; }
        public int Units { get; set; }
    }
}
