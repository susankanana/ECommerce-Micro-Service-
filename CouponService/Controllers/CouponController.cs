﻿using AutoMapper;
using CouponService.Models.Dtos;
using CouponService.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace CouponService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICoupon _couponService;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;
        public CouponController(ICoupon coupon, IMapper mapper)
        {
            _couponService = coupon;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]

        public async Task<ActionResult<ResponseDto>> GetAllCoupons()
        {
            var coupons = await _couponService.GetAllCoupons();
            _response.Result = coupons;
            return Ok(_response);
        }

        [HttpGet("single/{Id}")]

        public async Task<ActionResult<ResponseDto>> GetCoupon(Guid Id)
        {
            var coupon = await _couponService.GetCoupon(Id);
            if (coupon == null)
            {
                _response.ErrorMessage = "Coupon Not found";
                return NotFound(_response);
            }
            _response.Result = coupon;
            return Ok(_response);
        }

        [HttpGet("{Code}")]

        public async Task<ActionResult<ResponseDto>> GetCoupon(string Code)
        {
            var coupon = await _couponService.GetCoupon(Code);
            if (coupon == null)
            {
                _response.ErrorMessage = "Coupon Not found";
                return NotFound(_response);
            }
            _response.Result = coupon;
            return Ok(_response);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> AddCoupon(AddCouponDto newCoupon)
        {
            var coupon = _mapper.Map<Models.Coupon>(newCoupon);
            var response = await _couponService.AddCoupon(coupon);
            _response.Result = response;

            //add coupon to stripe

            var options = new CouponCreateOptions()  //pass options we will be needing to create a coupon
            {
                AmountOff = (long)newCoupon.CouponAmount * 100,
                Currency = "kes",
                Id = newCoupon.CouponCode, //a  a and b are similar to avoid the need to know what Id was given to coupon while deleting a coupon
                Name = newCoupon.CouponCode  //b
            };

            var service = new Stripe.CouponService();
            service.Create(options);

            return Created("", _response);
        }

        [HttpPut("{Id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> updateCoupon(Guid Id, AddCouponDto updCoupon)
        {
            var coupon = await _couponService.GetCoupon(Id);
            if (coupon == null)
            {
                _response.Result = "Coupon Not Found";
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            _mapper.Map(updCoupon, coupon);
            var res = await _couponService.UpdateCoupon();
            var service = new Stripe.CouponService();
            service.Delete(coupon.CouponCode);

            var options = new CouponCreateOptions()
            {
                AmountOff = (long)updCoupon.CouponAmount * 100,
                Currency = "kes",
                Id = updCoupon.CouponCode,
                Name = updCoupon.CouponCode
            };

            service.Create(options);


            _response.Result = res;
            return Ok(_response);
        }

        [HttpDelete("{Id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> deleteCoupon(Guid Id)
        {
            var coupon = await _couponService.GetCoupon(Id);
            if (coupon == null)
            {
                _response.Result = "Coupon Not Found";
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            var res = await _couponService.DeleteCoupon(coupon);

            var service = new Stripe.CouponService();
            service.Delete(coupon.CouponCode);  //id of coupon

            _response.Result = res;
            return Ok(_response);
        }
    }
}
