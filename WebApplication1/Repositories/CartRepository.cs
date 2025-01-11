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
        public async Task<int> AddProduct(int productId, int quantity)
        {
            string userId = GetUserId();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged in");
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
                    var product = _context.Products.Find(productId);
                    cartProduct = new CartDetail
                    {
                        ProductId = productId,
                        ShoppingCartId = cart.Id,
                        Quantity = quantity,
                        UnitPrice = product.Price
                    };
                    _context.CartDetails.Add(cartProduct);
                }
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            var cartProductCount = await GetCartProductCount(userId);
            return cartProductCount;
        }

        public async Task<int> RemoveProduct(int productId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged in");
                }
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    throw new Exception($"Could not find cart {userId}");
                }

                var cartProduct = _context.CartDetails
                    .FirstOrDefault(c => c.ShoppingCartId == cart.Id && c.ProductId == productId);
                if (cartProduct == null)
                {
                    throw new Exception("No proucts in cart");
                }
                else if (cartProduct.Quantity == 1)
                {
                    _context.CartDetails.Remove(cartProduct);
                }
                else
                {
                    cartProduct.Quantity--;
                }
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            var cartProductCount = await GetCartProductCount(userId);
            return cartProductCount;

        }

        public async Task<ShoppingCart> GetUserCart()
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
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return shoppingCart;
        }

        public async Task<bool> Checkout()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged in");
                }
                var cart = await GetCart(userId);

                if(cart == null)
                {
                    throw new Exception("Invalid cart");
                }
                var cartDetail = await _context.CartDetails
                    .Where(c => c.ShoppingCartId == cart.Id)
                    .ToListAsync();

                if(cartDetail.Count == 0)
                {
                    throw new Exception("Cart is empty");
                }
                var order = new Order
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    OrderStatusId = 1
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
                foreach(var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
                await _context.SaveChangesAsync();

                _context.CartDetails.RemoveRange(cartDetail);
                await _context.SaveChangesAsync();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        private async Task<ShoppingCart> GetCart(string userId)
        {
            var result = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return result;
        }

        public async Task<int> GetCartProductCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _context.ShoppingCarts
                              join cartDetail in _context.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              where cart.UserId == userId
                              select new { cartDetail.Id }).ToListAsync();
            return data.Count();
        }

        private string GetUserId()
        {
            ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;
            return _userManager.GetUserId(user);
        }
    }
}
