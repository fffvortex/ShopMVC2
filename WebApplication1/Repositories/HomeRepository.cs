using Microsoft.EntityFrameworkCore;

namespace ShopMVC2.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;
        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<double> GetMaxPrice()
        {
            return await _context.Products.MaxAsync(p => p.Price);
        }

        public async Task<IEnumerable<ProductType>> GetProductTypes()
        {
            return await _context.ProductTypes.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProducts(string search = "", int productTypeId = 0, double maxPrice = 0, double minPrice = 0)
        {
            search = search.ToLower().Replace(" ", "");

            var products = await (from product in _context.Products
                                  join type in _context.ProductTypes
                                  on product.ProductTypeId equals type.Id
                                  where string.IsNullOrEmpty(search) || (product != null && product.ProductTitle.ToLower().Replace(" ", "").Contains(search))
                                  select new Product
                                  {
                                      Id = product.Id,
                                      ProductImage = product.ProductImage,
                                      Description = product.Description,
                                      ProductTitle = product.ProductTitle,
                                      ProductTypeId = product.ProductTypeId,
                                      Price = product.Price,
                                      ProductTypeTitle = type.TypeTitle
                                  }).AsNoTracking().ToListAsync();
            if (productTypeId > 0)
            {
                products = products.Where(p => p.ProductTypeId == productTypeId).ToList();
            }
            if (maxPrice > 0)
            {
                products = products.Where(p => p.Price <= maxPrice).ToList();
            }
            if (minPrice > 0)
            {
                products = products.Where(p => p.Price >= minPrice).ToList();
            }

            return products;
        }

        public async Task<Product> GetProductById(int id)
        {
            var productById = await (from product in _context.Products
                                     join types in _context.ProductTypes on product.ProductTypeId equals types.Id
                                     where product.Id == id
                                     select new Product
                                     {
                                         Id = product.Id,
                                         ProductImage = product.ProductImage,
                                         Description = product.Description,
                                         ProductTitle = product.ProductTitle,
                                         ProductTypeId = product.ProductTypeId,
                                         Price = product.Price,
                                         ProductTypeTitle = types.TypeTitle
                                     }).AsNoTracking().FirstOrDefaultAsync();
            return productById;
        }
    }
}
