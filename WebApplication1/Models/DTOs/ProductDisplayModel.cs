namespace ShopMVC2.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<ProductType> ProductTypes { get; set; }

        public string Search { get; set; } = "";

        public int ProductTypeId { get; set; } = 0;

        public double MaxPrice { get; set; } = 0;

        public double MinPrice { get; set; } = 0;

        public double MaxPriceOfAllProducts { get; set; } = 0;
    }
}
