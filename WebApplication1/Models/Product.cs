using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMVC2.Models
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(45)]
        public string? ProductTitle { get; set; }

        public double Price { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        public string? ProductImage { get; set; }

        [Required]
        public int ProductTypeId { get; set; }

        public ProductType ProductType { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public List<CartDetail> CartDetails { get; set; }

        public Stock Stock { get; set; }

        [NotMapped]
        public string ProductTypeTitle { get; set; }

        [NotMapped]
        public int Quantity { get; set; }
    }
}
