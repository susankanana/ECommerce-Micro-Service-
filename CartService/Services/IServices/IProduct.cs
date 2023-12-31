using CartService.Models.Dtos;

namespace CartService.Services.IServices
{
    public interface IProduct
    {
        Task<ProductDto> GetProductById(Guid Id);
    }
}
