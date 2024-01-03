using OrderService.Models.Dtos;

namespace OrderService.Services.IServices
{
    public interface ICart
    {
        Task<CartDto> GetCartByUserId(Guid userId, string token);
        Task<bool> DeleteCart(Guid userId, string token);
    }
}
