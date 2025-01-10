
namespace ShopMVC2.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Product>> GetProducts(string search = "", int productTypeId = 0, double maxPrice = 0, double minPrice = 0);

        Task<IEnumerable<ProductType>> GetProductTypes();

        Task<double> GetMaxPrice();
    }
}