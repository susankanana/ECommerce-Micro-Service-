using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using ProductService.Services.IServices;
using static System.Net.Mime.MediaTypeNames;

namespace ProductService.Services
{
    public class ProductImageService : IProductImage
    {
        private readonly ApplicationDbContext _context;
        public ProductImageService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddImage(Guid Id, ProductImage prdImage)
        {
            //find the product by Id
            var product = await _context.Products.Where(x => x.ProductId == Id).FirstOrDefaultAsync();
            product.ProductImages.Add(prdImage);
            await _context.SaveChangesAsync();
            return "Image Added!!!";
        }
    }
}
