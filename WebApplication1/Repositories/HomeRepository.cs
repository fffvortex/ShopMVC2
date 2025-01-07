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
        public async Task<IEnumerable<Product>> GetProducts(string search = "", int productTypeId = 0)
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
                                  }).ToListAsync();
            if (productTypeId > 0)
            {
                products = products.Where(p => p.ProductTypeId == productTypeId).ToList();
            }

            return products;
        }
    }
}
