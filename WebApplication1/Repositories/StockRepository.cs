using Microsoft.EntityFrameworkCore;

namespace ShopMVC2.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public StockRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Stock?> GetStockByProductId(int productId)
            => await _applicationDbContext.Stocks
            .FirstOrDefaultAsync(s => s.ProductId == productId);

        public async Task<int> GetQuantityInStockByProductId(int productId)
        {
            var stock = await _applicationDbContext.Stocks.FirstOrDefaultAsync(s => s.ProductId == productId);
            int quantity = stock == null ? 0 : stock.Quantity;
            return quantity;
        }

        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string search = "", int id = 0, string sortByQuantity = "")
        {
            var stocks = await (from product in _applicationDbContext.Products
                                join stock in _applicationDbContext.Stocks
                                on product.Id equals stock.ProductId
                                into product_stock
                                from productStock in product_stock.DefaultIfEmpty()
                                where string.IsNullOrWhiteSpace(search) ||
                                product.ProductTitle.ToLower().Replace(" ", "").Contains(search.ToLower().Replace(" ", ""))
                                select new StockDisplayModel
                                {
                                    ProductId = product.Id,
                                    ProductTitle = product.ProductTitle,
                                    Quantity = productStock == null ? 0 : productStock.Quantity
                                }).OrderBy(s => s.ProductId).ToListAsync();
            if (id != 0)
            {
                stocks = stocks.Where(s => s.ProductId == id).ToList();
            }
            if (sortByQuantity == "quantity")
            {
                stocks = stocks.OrderBy(s => s.Quantity).ToList();
            }
            if (sortByQuantity == "descquantity")
            {
                stocks = stocks.OrderByDescending(s => s.Quantity).ToList();
            }

            return stocks;
        }

        public async Task ManageStock(StockDTO stockToManage)
        {
            var existingStock = await GetStockByProductId(stockToManage.ProductId);
            if (existingStock == null)
            {
                var stock = new Stock
                {
                    ProductId = stockToManage.ProductId,
                    Quantity = stockToManage.Quantity
                };
                await _applicationDbContext.Stocks.AddAsync(stock);
            }
            else
            {
                existingStock.Quantity = stockToManage.Quantity;
            }
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
