using BlazorECommerce.Models;
using static System.Net.WebRequestMethods;

namespace BlazorECommerce.Services.IService
{
    public interface ICart
    {
        Task<ResponseDto> AddProductToCart(AddCartItemDto cartItemDto);
        Task<Cart> GetCartByUserId(Guid userId);

        Task<ResponseDto> ApplyCoupon(Cart cart, string code);


        Task<ResponseDto> RemoveProductFromCart(Guid productId);

        Task<ResponseDto> ReduceCartItemQuantity(RemoveFromCartDto item);
    }
}
