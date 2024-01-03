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

        public async Task<List<CartAndCartItemsResponseDto>> GetCarts()
        {
            return await _context.Carts.Select(c => new CartAndCartItemsResponseDto()
            {
                CartId = c.CartId,
                CouponDiscount = c.CouponDiscount,
                CouponCode = c.CouponCode,
                UserId = c.UserId,
                Items = c.CartItems.Select(x => new CartItemResponseDto()
                {
                    ProductName = x.ProductName,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    CartItemId = x.CartItemId,
                }).ToList()
            }).ToListAsync();
        }

        public async Task<Cart> GetCartById(Guid Id)
        {
            return await _context.Carts.Where(x => x.CartId == Id).FirstOrDefaultAsync();
        }

        public async Task<CartAndCartItemsResponseDto> GetCartByUserId(Guid Id)
        {
            return await _context.Carts.Include(cart => cart.CartItems).Where(x => x.UserId == Id).Select(c => new CartAndCartItemsResponseDto()
            {
                CartId = c.CartId,
                CouponDiscount = c.CouponDiscount,
                CouponCode = c.CouponCode,
                CartTotal= c.CartTotal,
                UserId = c.UserId,
                Items = c.CartItems.Select(x => new CartItemResponseDto()
                {
                    ProductName = x.ProductName,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    CartItemId = x.CartItemId,
                    ProductPrice=x.ProductPrice

                }).ToList()
            }).FirstOrDefaultAsync();
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
        public async Task UpdateCartTotals(Guid userId , double total)
        {
            var cart = await _context.Carts.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            cart.CartTotal = total;
        }

        public async Task ApplyCoupon(Guid userId, string couponCode, int couponAmount)
        {
            var cart = await _context.Carts.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            cart.CouponCode = couponCode;
            cart.CouponDiscount = couponAmount;
        }



        //public async Task<Cart> GetCartByUserId(Guid Id)
        //{
        //    return await _context.Carts.Where(x => x.UserId == Id).FirstOrDefaultAsync();
        //}
    }
}
