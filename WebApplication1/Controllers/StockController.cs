using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopMVC2.Constants;

namespace ShopMVC2.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class StockController : Controller
    {
        private readonly IStockRepository _stockRepository;
        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task<IActionResult> Stocks(string search = "", int id = 0, string sortByQuantity = "")
        {
            var stocks = await _stockRepository.GetStocks(search, id, sortByQuantity);
            return View(stocks);
        }

        public async Task<IActionResult> ManageStock(int productId)
        {
            var existingStock = await _stockRepository.GetStockByProductId(productId);
            var stock = new StockDTO
            {
                ProductId = productId,
                Quantity = existingStock != null ? existingStock.Quantity : 0
            };
            return View(stock);
        }
        [HttpPost]
        public async Task<IActionResult> ManageStock(StockDTO stock)
        {
            if (!ModelState.IsValid)
            {
                return View(stock);
            }
            try
            {
                await _stockRepository.ManageStock(stock);
                TempData["successMessage"] = "Stock is updated successfully";
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Stock update is failed";
            }
            return RedirectToAction(nameof(Stocks));
        }
    }
}
