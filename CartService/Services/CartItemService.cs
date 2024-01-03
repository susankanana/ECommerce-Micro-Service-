using CartService.Data;
using CartService.Models;
using CartService.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CartService.Services
{
    public class CartItemService : ICartItem
    {
        private readonly ApplicationDbContext _context;
        public CartItemService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddToCart(Guid Id, CartItem item)
        {
            //get cart by Id
            var cart = await _context.Carts.Where(x => x.CartId == Id).FirstOrDefaultAsync();
            cart.CartItems.Add(item);
            await _context.SaveChangesAsync();
            return "Ïtem added to cart !!";
        }
        public async Task UpdateCartItemQuantity(Guid cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems.Where(x => x.CartItemId == cartItemId).FirstOrDefaultAsync();
            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartItem>> GetCartItems(Guid cartId)
        {
            return await _context.CartItems.Where(x => x.CartId == cartId).ToListAsync();
        }

        public async Task<CartItem> GetCartItemById(Guid cartItemId)
        {
            return await _context.CartItems.Where(x => x.CartItemId == cartItemId).FirstOrDefaultAsync();
        }

        public async Task<CartItem> GetCartItemByProductId(Guid productId)
        {
            return await _context.CartItems.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task<bool> RemoveProductFromCart(Guid productId)
        {
            var cartItem = await _context.CartItems.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        
    }
}
