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

        public async Task ChangeOrderStatus(UpdateOrderStatusModel data)
        {
            var order = await _applicationDbContext.Orders.FindAsync(data.OrderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with id:{data.OrderStatusId} does not found");
            }
            order.OrderStatusId = data.OrderStatusId;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderById(int orderId)
        {
            return await _applicationDbContext.Orders.FindAsync(orderId);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await _applicationDbContext.OrderStatuses.ToListAsync();
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _applicationDbContext.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with id:{orderId} does not found");
            }
            order.IsPaid = !order.IsPaid;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> UserOrders(bool getAll)
        {
            var orders = _applicationDbContext.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductType)
                .AsQueryable();

            if (!getAll)
            {
                var userId = _cartRepository.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged in");
                }
                orders = orders.Where(o => o.UserId == userId);
                return await orders.AsNoTracking().ToListAsync();
            }
            return await orders.AsNoTracking().ToListAsync();
        }
    }
}
