using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Order.API.Core.Models.Domain
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        public bool Acitve { get; set; }

        public ICollection<OrderDetailModel> OrderDetails { get; set; }
    }
}
