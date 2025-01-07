
namespace ShopMVC2.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Product>> GetProducts(string search = "", int productTypeId = 0);
    }
}