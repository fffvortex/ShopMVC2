﻿
namespace ShopMVC2.Repositories
{
    public interface ICartRepository
    {
        Task<int> GetProductQuantityInCartByProductId(int productId);
        Task<int> AddProduct(int productId, int quantity);
        Task<int> RemoveProduct(int productId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartProductCount(string userId = "");
        Task<bool> Checkout(CheckoutModel model);
        string GetUserId();
    }
}