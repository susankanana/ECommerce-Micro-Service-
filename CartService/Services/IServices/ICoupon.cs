using CartService.Models.Dtos;

namespace CartService.Services.IServices
{
    public interface ICoupon
    {
        Task<CouponDto> GetCouponByCouponCode(string couponCode);
    }
}
