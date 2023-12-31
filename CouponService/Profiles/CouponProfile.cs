using AutoMapper;
using CouponService.Models.Dtos;
using CouponService.Models;

namespace CouponService.Profiles
{
    public class CouponProfile : Profile
    {

        public CouponProfile()
        {
            CreateMap<AddCouponDto, Coupon>().ReverseMap();
        }
    }
}
