using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string search = "", int productTypeId = 0, double maxPrice = 0, double minPrice = 0)
        {
            IEnumerable<Product> products = await _homeRepository.GetProducts(search, productTypeId, maxPrice, minPrice);
            IEnumerable<ProductType> types = await _homeRepository.GetProductTypes();
            double maxPriceOfAllProducts = await _homeRepository.GetMaxPrice();
            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Products = products,
                ProductTypes = types,
                ProductTypeId = productTypeId,
                Search = search,
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                MaxPriceOfAllProducts=maxPriceOfAllProducts
            };
            return View(productModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
