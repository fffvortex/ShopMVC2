
namespace ShopMVC2.Repositories
{
    public interface ICartRepository
    {
        Task<bool> AddProduct(int productId, int quantity);
        Task<IEnumerable<ShoppingCart>> GetUserCart();
        Task<bool> RemoveProduct(int productId);
    }
}