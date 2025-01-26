
namespace ShopMVC2.Repositories
{
    public interface IProductTypeRepository
    {
        Task AddProductType(ProductType productTypeToAdd);
        Task DeleteProductType(ProductType productTypeToDelete);
        Task<ProductType?> GetProductTypeById(int id);
        Task<IEnumerable<ProductType>> GetProductTypes();
        Task UpdateProductType(ProductType productTypeToUpdate);
    }
}