using CartService.Data;
using CartService.Models;
using CartService.Models.Dtos;
using CartService.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CartService.Services
{
    public class CartsService : ICart
    {
        private readonly ApplicationDbContext _context;
        public CartsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartAndCartItemsResponseDto>> GetCart()
        {
            return await _context.Carts.Select(c => new CartAndCartItemsResponseDto()
            {
                CartId = c.CartId,
                CouponDiscount = c.CouponDiscount,
                CouponCode = c.CouponCode,
                Items = c.CartItems.Select(x => new CartItemResponseDto()
                {
                    ProductName = x.ProductName
                }).ToList()
            }).ToListAsync();
        }

        public async Task<Cart> GetCartById(Guid Id)
        {
            return await _context.Carts.Where(x => x.CartId == Id).FirstOrDefaultAsync();
        }

        public async Task<string> AddCart(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return "Cart added successfully";
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCartByUserId(Guid Id)
        {
            return await _context.Carts.Where(x => x.UserId == Id).FirstOrDefaultAsync();
        }
    }
}
