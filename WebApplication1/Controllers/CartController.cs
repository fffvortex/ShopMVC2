using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShopMVC2.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<IActionResult> AddProduct(int productId, int quantity = 1, int redirect = 0)
        {
            var cartCount = await _cartRepository.AddProduct(productId, quantity);

            if(redirect == 0)
            {
                return Ok(cartCount);
            }
            return RedirectToAction(nameof(GetUserCart));
        }
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            var cartCount = await _cartRepository.RemoveProduct(productId);

            return RedirectToAction(nameof(GetUserCart));
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalProductInCart()
        {
            int cartProductCount = await _cartRepository.GetCartProductCount();
            return Ok(cartProductCount);
        }
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var isChecked = await _cartRepository.Checkout(model);
            if (!isChecked)
            {
                return RedirectToAction(nameof(OrderFailure));
            }
            TempData["OrderName"] = model.Name;
            return RedirectToAction(nameof(OrderSuccess));
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
        public IActionResult OrderFailure()
        {
            return View();
        }

        public async Task<int> GetProductQuantityInCartByProductId(int productId)
        {
            return await _cartRepository.GetProductQuantityInCartByProductId(productId);
        }
    }
}
