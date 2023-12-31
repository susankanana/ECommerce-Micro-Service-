using ProductService.Models;

namespace ProductService.Services.IServices
{
    public interface IProductImage
    {
        Task<string> AddImage(Guid Id, ProductImage prdImage);
    }
}
