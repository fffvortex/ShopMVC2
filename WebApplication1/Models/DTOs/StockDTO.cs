using System.ComponentModel.DataAnnotations;

namespace ShopMVC2.Models.DTOs
{
    public class StockDTO
    {
        public int ProductId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity nust be a non-negative value")]

        public int Quantity { get; set; }
    }
}