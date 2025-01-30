using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeRepository _homeRepository;
        public HomeController(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string search = "", int productTypeId = 0, double maxPrice = 0, double minPrice = 0, int currentPage = 1)
        {
            IEnumerable<Product> products = await _homeRepository.GetProducts(search, productTypeId, maxPrice, minPrice, currentPage);
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
                MaxPriceOfAllProducts = maxPriceOfAllProducts,
                CurrentPage = currentPage,
                PageSize = _homeRepository.GetPagesize(),
                TotalPages = _homeRepository.GetTotalPages()
            };
            return View(productModel);
        }

        public async Task<IActionResult> IndexSingle(int id)
        {
            var product = await _homeRepository.GetProductById(id);
            return View(product);
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
