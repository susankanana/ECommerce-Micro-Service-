using AutoMapper;
using CartService.Models;
using CartService.Models.Dtos;
using CartService.Services;
using CartService.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartItem _cartItemService;
        private readonly ICoupon _couponService;
        private readonly ICart _cartService;
        private readonly ResponseDto _response;
        public CartController(ICart cart, ICoupon coupon, ICartItem item)
        {
            _cartItemService = item;
            _couponService = coupon;
            _cartService = cart;
            _response = new ResponseDto();
        }

        [HttpGet]
        [Authorize]

        public async Task<ActionResult<ResponseDto>> GetExistingCarts()
        {

            var res = await _cartService.GetCarts();
            _response.Result = res;
            return Ok(_response);

        }


        [HttpGet("view/{UserId}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto>> ViewCart(Guid UserId)
        {
            var cart = await _cartService.GetCartByUserId(UserId);

            if (cart == null || cart.Items.Count == 0)
            {
                _response.Result = "Your Cart is Empty !!";
                return Ok(_response);
            }
            _response.Result = cart;
            return Ok(_response);

        }

        [HttpPut("totals/{UserId}")]

        public async Task<ActionResult<ResponseDto>> UpdateCartTotals(Guid UserId)
        {

            var cart = await _cartService.GetCartByUserId(UserId);
            if (cart == null)
            {
                _response.ErrorMessage = "Cart not found";
                return NotFound(_response);
            }
            var items = await _cartItemService.GetCartItems(cart.CartId);
            var total = 0;
            foreach (var item in items)
            {
                total += item.ProductPrice * item.Quantity;
            }
           cart.CartTotal = total;
            await _cartService.UpdateCartTotals(UserId, total);
           await _cartService.SaveChanges();

            _response.Result = cart;
            return Ok(_response);


        }
        [HttpPut("coupon/{UserId}")]

        public async Task<ActionResult<ResponseDto>> ApplyCoupon(Guid UserId, string Code)
        {
            //get the cart and coupon
            var cart = await _cartService.GetCartByUserId(UserId);
            if (cart == null)
            {
                _response.ErrorMessage = "Cart Not Found";
                return NotFound(_response);
            }
            var coupon = await _couponService.GetCouponByCouponCode(Code);
            if (coupon == null)
            {
                _response.ErrorMessage = "Coupon is not Valid";
                return NotFound(_response);
            }
            if (!string.IsNullOrWhiteSpace(cart.CouponCode))
            {
                _response.ErrorMessage = "You already have a coupon applied!";
                return BadRequest(_response);
            }

            if (coupon.CouponMinAmount <= cart.CartTotal)
            {
                cart.CouponCode = coupon.CouponCode;
                cart.CouponDiscount = coupon.CouponAmount;
                await _cartService.ApplyCoupon(UserId, coupon.CouponCode, coupon.CouponAmount);
                await _cartService.SaveChanges();
                _response.Result = "Code applied";
                return Ok(_response);
            }
            else
            {
                _response.ErrorMessage = "Total amount is less that the minimum amount for this coupon";
                return BadRequest(_response);
            }

        }
    }
}
