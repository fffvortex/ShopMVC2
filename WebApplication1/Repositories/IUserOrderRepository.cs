namespace ShopMVC2.Repositories
{
    public interface IUserOrderRepository
    {
        Task<List<Order>> UserOrders();
    }
}