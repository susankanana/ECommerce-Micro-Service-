using ProductService.Models;
using ProductService.Models.Dtos;

namespace ProductService.Services.IServices
{
    public interface IProduct
    {
        Task<List<ProductsAndImagesResponseDto>> GetProducts();
        Task<Product> GetProductById(Guid Id);
        Task<string> AddProduct(Product product);
        Task<string> DeleteProduct(Product product);
        Task<string> UpdateProduct();


    }
}
