using Microsoft.EntityFrameworkCore;

namespace ShopMVC2.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ICartRepository _cartRepository;
        public UserOrderRepository(ApplicationDbContext applicationDbContext, ICartRepository cartRepository)
        {
            _applicationDbContext = applicationDbContext;
            _cartRepository = cartRepository;
        }
        public async Task<List<Order>> UserOrders()
        {
            var userId = _cartRepository.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User is not logged in");
            }
            var orders = await _applicationDbContext.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductType)
                .Where(o => o.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            return orders;
        }
    }
}
