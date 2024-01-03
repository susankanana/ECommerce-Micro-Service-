using CartService.Models;

namespace CartService.Services.IServices
{
    public interface ICartItem
    {
        Task<string> AddToCart(Guid Id, CartItem item);
        Task<bool> RemoveProductFromCart(Guid productId);
        Task<List<CartItem>> GetCartItems(Guid cartId);
        Task<CartItem> GetCartItemById(Guid cartItemId);
        Task<CartItem> GetCartItemByProductId(Guid productId);
        Task UpdateCartItemQuantity(Guid cartItemId, int quantity);

    }
}
