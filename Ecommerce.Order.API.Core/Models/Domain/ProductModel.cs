using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Order.API.Core.Models.Domain
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int AvailableStock { get; set; }

        [Required]
        public int MaxStockThreshold { get; set; }

        public ICollection<OrderModel> Orders { get; set; }
    }
}