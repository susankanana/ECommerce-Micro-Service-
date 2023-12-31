using OrderService.Models.Dtos;

namespace OrderService.Services.IServices
{
    public interface ICart
    {
        Task<CartDto> GetCartById(Guid cartId);
    }
}
