using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ecommerce.Order.API.Core.Models.Domain
{
    public class OrderDetailModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int Units { get; set; }

        public ProductModel Product { get; set; }

        [JsonIgnore]
        public OrderModel Order { get; set; }
    }
}
