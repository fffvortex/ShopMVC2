
namespace ShopMVC2.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Product>> GetProducts(string search = "", int productTypeId = 0, int maxPrice = 0, int minPrice = 0);

        Task<IEnumerable<ProductType>> GetProductTypes();
    }
}