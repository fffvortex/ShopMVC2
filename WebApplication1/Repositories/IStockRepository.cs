
namespace ShopMVC2.Repositories
{
    public interface IStockRepository
    {
        Task<Stock?> GetStockByProductId(int productId);
        Task<IEnumerable<StockDisplayModel>> GetStocks(string search = "", int id = 0, string sortByQuantity = "");
        Task ManageStock(StockDTO stockToManage);
    }
}