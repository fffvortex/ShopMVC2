using Microsoft.EntityFrameworkCore;

namespace ShopMVC2.Repositories
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProductType(ProductType productTypeToAdd)
        {
            _context.ProductTypes.Add(productTypeToAdd);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateProductType(ProductType productTypeToUpdate)
        {
            _context.ProductTypes.Update(productTypeToUpdate);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProductType(ProductType productTypeToDelete)
        {
            _context.ProductTypes.Remove(productTypeToDelete);
            await _context.SaveChangesAsync();
        }
        public async Task<ProductType?> GetProductTypeById(int id)
        {
            return await _context.ProductTypes.FindAsync(id);
        }
        public async Task<IEnumerable<ProductType>> GetProductTypes()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}
