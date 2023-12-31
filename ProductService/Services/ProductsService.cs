using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using ProductService.Models.Dtos;
using ProductService.Services.IServices;

namespace ProductService.Services
{
    public class ProductsService : IProduct
    {
        private readonly ApplicationDbContext _context;
        public ProductsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return "Product Added!!";
        }

        public async Task<string> DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return "Product Removed!!";
        }

        public async Task<Product> GetProductById(Guid Id)
        {
            return await _context.Products.Where(x => x.ProductId == Id).FirstOrDefaultAsync();
        }

        public async Task<List<ProductsAndImagesResponseDto>> GetProducts()
        {

            return await _context.Products.Select(p => new ProductsAndImagesResponseDto()
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductPrice = p.ProductPrice,
                CreatedDate = p.CreatedDate,
                Availability = p.Availability,
                ProductImagesDtos = p.ProductImages.Select(x => new AddProductImageDto()
                {
                    Image = x.Image
                }).ToList()
            }).ToListAsync();
            
        }

        public async Task<string> UpdateProduct()
        {
            await _context.SaveChangesAsync();
            return "Product Updated!!";
        }
    }
}
