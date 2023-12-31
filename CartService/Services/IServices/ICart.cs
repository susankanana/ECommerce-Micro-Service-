using CartService.Models;
using CartService.Models.Dtos;

namespace CartService.Services.IServices
{
    public interface ICart
    {
        Task<List<CartAndCartItemsResponseDto>> GetCart();
        Task<Cart> GetCartById(Guid Id);
        Task<Cart> GetCartByUserId(Guid Id);
        Task<string> AddCart(Cart cart);
        Task SaveChanges();
    }
}
