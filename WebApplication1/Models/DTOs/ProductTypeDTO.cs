using System.ComponentModel.DataAnnotations;

namespace ShopMVC2.Models.DTOs
{
    public class ProductTypeDTO
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string ProductTypeTitle { get; set; }
    }
}
