using OrderService.Models.Dtos;

namespace OrderService.Services.IServices
{
    public interface ICartItem
    {
        Task<CartItemDto> GetCartItemById(Guid cartItemId);
    }
}
