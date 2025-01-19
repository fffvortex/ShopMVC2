namespace ShopMVC2.Models.DTOs
{
    public class OrderDetailModel
    {
        public string DivId { get; set; }

        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}
