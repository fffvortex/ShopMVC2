using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMVC2.Models
{
    [Table("ProductType")]
    public class ProductType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeTitle { get; set; }

        public List<Product> Products { get; set; }
    }
}
