using AutoMapper;
using CartService.Models;
using CartService.Models.Dtos;
using CartService.Services;
using CartService.Services.IServices;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly IProduct _productService;
        private readonly ICartItem _cartItemService;
        private readonly ICart _cartService;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;
        public CartItemController(ICart cart, ICartItem item, IMapper mapper, IProduct prd)
        {
            _productService = prd;
            _cartService = cart;
            _mapper = mapper;
            _cartItemService = item;
            _response = new ResponseDto();

        }

        [HttpPost]
        
        public async Task<ActionResult<ResponseDto>> AddProductToCart(CartRequestDto cartRequest)
        {
            //var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            // check if product exists 
            var product = await _productService.GetProductById(cartRequest.item.ProductId);
            if (product == null)
            {
                _response.ErrorMessage = "The product you are trying to add does not exist !";
                return NotFound(_response);
            }
            //product exists
            if (cartRequest.item.Quantity > product.Stock)
            {
                _response.ErrorMessage = "The quantity you want exceeds the available stock !";
                return BadRequest(_response);
            }
            //check if user has already added a product to cart
            var cart = await _cartService.GetCartByUserId(cartRequest.dto.UserId);

            if (cart == null) //first time user
            {
                //create cart and cart items
                var _cart = _mapper.Map<Cart>(cartRequest.dto);
                //_cart.UserId = cartRequest.dto.UserId;
                var resp = await _cartService.AddCart(_cart);
                _response.Result = resp;

                var _item = _mapper.Map<CartItem>(cartRequest.item);
                var ress = await _cartItemService.AddToCart(_cart.CartId, _item);
                _response.Result = ress;
                return Created("", _response);
                // return Ok(_response);


            }
            else
            {
                //check if productId in this cart is in the cart items table. i.e if it is a new product or an existing product.
                var cartProduct = cart.CartItems.Find(x => x.ProductId == product.ProductId);
                if (cartProduct == null) //either a new product or cart has no product
                {
                    if (cartRequest.item.Quantity == 0)//empty cart
                    {
                        _response.ErrorMessage = "You must at least add one product to the cart !";
                        return BadRequest(_response);
                    }
                    //new product
                    var cartItem = new CartItem() // make the product a cart item
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductPrice = product.ProductPrice,
                        Quantity = cartRequest.item.Quantity,
                        CartId = cart.CartId
                    };
                    await _cartItemService.AddToCart(cart.CartId, cartItem);
                }
                else //existing product
                {
                    var newQuantity = cartProduct.Quantity + cartRequest.item.Quantity;
                    if (newQuantity > product.Stock)
                    {
                        _response.ErrorMessage = "The quantity you want exceeds the available stock !";
                        return BadRequest(_response);
                    }
                    await _cartItemService.UpdateCartItemQuantity(cartProduct.ProductId, newQuantity);
                }
                _response.Result = "Product Added To Cart Successfully !";
                return Ok(_response);
            }
        }

        [HttpDelete("{ProductId}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto>> RemoveProductFromCart(Guid ProductId)
        {
            var isProductRemoved = await _cartItemService.RemoveProductFromCart(ProductId);
            if (!isProductRemoved)
            {
                _response.ErrorMessage = "The product you are trying to remove does not exist in your cart !";
                return NotFound(_response);
            }
            var product = await _productService.GetProductById(ProductId);
            _response.Result = $"{product.ProductName} has been removed from your cart";
            return Ok(_response);

        }

        [HttpPatch]
        public async Task<ActionResult<ResponseDto>> ReduceCartItemQuantity(RemoveFromCartDto item)
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _response.ErrorMessage = "You are not authorized";
                return StatusCode(403, _response);
            }

            var cart = await _cartService.GetCartByUserId(new Guid(userId));
            if (cart == null || cart.CartItems.Count == 0)
            {
                _response.Result = "Your Cart is Empty!";
                return Ok(_response);
            }
            var cartItem = cart.CartItems.Find(x => x.ProductId == item.ProductId);

            if (cartItem == null)
            {
                _response.ErrorMessage = "The product you want to remove is not in your cart!";
                return NotFound(_response);
            }
            else
            {
                await _cartItemService.RemoveProductFromCart(item.ProductId);
                var product = await _productService.GetProductById(item.ProductId);
                _response.Result = $"{product.ProductName} has been removed from your cart";
                //return Ok(_response);
            }

            // ensure new quantity is set to zero whenever it is < 0
            var newQuantity = Math.Max(0, cartItem.Quantity - item.ProductQuantity);

            await _cartItemService.UpdateCartItemQuantity(cartItem.CartItemId, newQuantity);
            _response.Result = "Item quantity Updated Successfully !!";

            return Ok(_response);

        }

    }
}
