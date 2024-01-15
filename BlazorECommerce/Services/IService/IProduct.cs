using BlazorECommerce.Models;

namespace BlazorECommerce.Services.IService
{
    public interface IProduct
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(Guid Id);
        Task<ResponseDto> AddProduct(ProductRequestDto productRequestDto);
        Task<ResponseDto> DeleteProduct(Guid Id);
        Task<ResponseDto> UpdateProduct(Guid id, ProductRequestDto productRequestDto);
        
    }
}
