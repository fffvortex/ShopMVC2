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
            return RedirectToAction(nameof(GetUserCart)); //nameof
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
    }
}
