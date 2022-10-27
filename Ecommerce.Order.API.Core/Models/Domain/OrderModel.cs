using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Order.API.Core.Models.Domain
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Units { get; set; }

        public ICollection<ProductModel> Products { get; set; }
    }
}
