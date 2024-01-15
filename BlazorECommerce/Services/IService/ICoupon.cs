using BlazorECommerce.Models;
namespace BlazorECommerce.Services.IService
{
    public interface ICoupon
    {
        Task<List<Coupon>> GetAllCoupons();

        Task<Coupon> GetCoupon(string code);

        Task<ResponseDto> AddCoupon(AddCouponDto newCoupon);

        Task<ResponseDto> UpdateCoupon(Guid Id, AddCouponDto updCoupon);

        Task<ResponseDto> DeleteCoupon(Guid Id);
    }
}
