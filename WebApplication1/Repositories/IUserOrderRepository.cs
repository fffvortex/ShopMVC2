﻿namespace ShopMVC2.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders(bool getAll = false);

        Task ChangeOrderStatus(UpdateOrderStatusModel data);

        Task TogglePaymentStatus(int orderId);

        Task<Order?> GetOrderById(int orderId);

        Task<IEnumerable<OrderStatus>> GetOrderStatuses();
    }
}