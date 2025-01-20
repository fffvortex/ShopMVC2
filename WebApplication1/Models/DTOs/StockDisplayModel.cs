namespace ShopMVC2.Models.DTOs
{
    public class StockDisplayModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string? ProductTitle { get; set; }
    }
}
