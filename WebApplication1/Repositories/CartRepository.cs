using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ShopMVC2.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> AddProduct(int productId, int quantity)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                string userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return false;
                }
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _context.ShoppingCarts.Add(cart);
                }
                _context.SaveChanges();
                var cartProduct = _context.CartDetails
                    .FirstOrDefault(c => c.ShoppingCartId == cart.Id && c.ProductId == productId);
                if (cartProduct != null)
                {
                    cartProduct.Quantity += quantity;
                }
                else
                {
                    cartProduct = new CartDetail
                    {
                        ProductId = productId,
                        ShoppingCartId = cart.Id,
                        Quantity = quantity
                    };
                    _context.CartDetails.Add(cartProduct);
                }
                _context.SaveChanges();
                transaction.Commit();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> RemoveProduct(int productId)
        {
            try
            {
                string userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return false;
                }
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    return false;
                }

                var cartProduct = _context.CartDetails
                    .FirstOrDefault(c => c.ShoppingCartId == cart.Id && c.ProductId == productId);
                if (cartProduct == null)
                {
                    return false;
                }
                else if (cartProduct.Quantity == 1)
                {
                    _context.CartDetails.Remove(cartProduct);
                }
                else
                {
                    cartProduct.Quantity = cartProduct.Quantity - 1;
                }
                _context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<IEnumerable<ShoppingCart>> GetUserCart()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Invalid userId");
            }
            var shoppingCart = await _context.ShoppingCarts
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Product)
                .ThenInclude(a => a.ProductType)
                .Where(a => a.UserId == userId)
                .ToListAsync();
            return shoppingCart;
        }

        private async Task<ShoppingCart> GetCart(string userId)
        {
            var result = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return result;
        }

        private string GetUserId()
        {
            ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(user);
            return userId;
        }
    }
}
