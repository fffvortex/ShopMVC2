namespace ShopMVC2.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<ProductType> ProductTypes { get; set; }

        public string Search { get; set; } = "";
        public int ProductTypeId { get; set; } = 0;

        public int MaxPrice { get; set; } = 0;

        public int MinPrice { get; set; } = 0;
    }
}
