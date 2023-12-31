using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models.Dtos;
using OrderService.Services;
using OrderService.Services.IServices;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ResponseDto _responseDto;
        private readonly IOrder _orderService;
        public OrderController(IOrder order)
        {
            _orderService = order;
            _responseDto = new ResponseDto();
        }
        [HttpPost("Pay")]

        public async Task<ActionResult<ResponseDto>> MakePayments(StripeRequestDto dto)
        {

            var sR = await _orderService.MakePayments(dto);

            _responseDto.Result = sR;
            return Ok(_responseDto);
        }

        [HttpPost("validate/{Id}")]

        public async Task<ActionResult<ResponseDto>> ValidatePayment(Guid Id)
        {

            var res = await _orderService.ValidatePayments(Id);

            if (res)
            {
                _responseDto.Result = res;
                return Ok(_responseDto);
            }

            _responseDto.ErrorMessage = "Payment Failed!";
            return BadRequest(_responseDto);
        }
    }
}
