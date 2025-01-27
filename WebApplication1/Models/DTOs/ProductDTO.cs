using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ShopMVC2.Models.DTOs
{
    public class ProductDTO
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

        public IFormFile? ImageFile { get; set; }

        public IEnumerable<SelectListItem>? ProductTypeList { get; set; }

        public int Quantity { get; set; } = 0;
    }
}
