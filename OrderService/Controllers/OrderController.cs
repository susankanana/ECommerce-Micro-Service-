using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Models.Dtos;
using OrderService.Services;
using OrderService.Services.IServices;
using System.Security.Claims;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly ICart _cartService;
        private readonly IOrder _orderService;
        public OrderController(IOrder order , ICart cartService)
        {
            _orderService = order;
            _response = new ResponseDto();
            _cartService = cartService;

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto>> MakeOrder()
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (userId == null)
            {
                _response.ErrorMessage = "You are not authorized. Log in first.";
                return StatusCode(500, _response);
            }
            var cart = await _cartService.GetCartByUserId(new Guid(userId), token);

            if (cart == null || cart.CartTotal == 0)
            {
                _response.ErrorMessage = "Your cart is empty!";
                return BadRequest(_response);
            }

            var order = new Order()
            {
                UserId = new Guid(userId),
                OrderTotal = cart.CartTotal,
                CouponCode = cart.CouponCode,
                CouponDiscount = cart.CouponDiscount
            };

            var response = await _orderService.MakeOrder(order);
            _response.Result = response;
            return Ok(_response);
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetOrderByUserId()
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _response.ErrorMessage = "You are not authorized. Log in first.";
                return StatusCode(500, _response);
            }
            var order = await _orderService.GetOrderByUserId(new Guid(userId));

            if (order == null)
            {
                _response.ErrorMessage = "You have not placed an order yet .";
                return NotFound(_response);
            }

            _response.Result = order;
            return Ok(_response);
        }

        [HttpPost("Pay")]
        [Authorize]

        public async Task<ActionResult<ResponseDto>> MakePayments(StripeRequestDto dto)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (token == null)
            {
                _response.ErrorMessage = "You are not authorized. Log in first";
                return StatusCode(500, _response);
            }

            var sR = await _orderService.MakePayments(dto, token);

            _response.Result = sR;
            return Ok(_response);
        }

        [HttpPost("validate/{orderId}")]
        [Authorize]

        public async Task<ActionResult<ResponseDto>> ValidatePayment(Guid orderId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (token == null)
            {
                _response.ErrorMessage = "You are not authorized. Log in first.";
                return StatusCode(500, _response);
            }
            var res = await _orderService.ValidatePayments(orderId, token);

            if (res)
            {
                _response.Result = res;
                return Ok(_response);
            }

            _response.ErrorMessage = "Payment Failed!";
            return BadRequest(_response);
        }
    }
}
