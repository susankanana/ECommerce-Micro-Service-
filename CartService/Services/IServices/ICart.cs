using CartService.Models;
using CartService.Models.Dtos;

namespace CartService.Services.IServices
{
    public interface ICart
    {
        Task<List<CartAndCartItemsResponseDto>> GetCarts();
        Task<Cart> GetCartById(Guid Id);
        Task<CartAndCartItemsResponseDto> GetCartByUserId(Guid Id);
        Task<string> AddCart(Cart cart);
        Task SaveChanges();
        Task UpdateCartTotals(Guid userId, double total);
        Task ApplyCoupon(Guid userId, string couponCode, int couponAmount);
    }
}
