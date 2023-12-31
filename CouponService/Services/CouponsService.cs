using CouponService.Data;
using CouponService.Models;
using CouponService.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CouponService.Services
{
    public class CouponsService : ICoupon
    {
        private readonly ApplicationDbContext _context;
        public CouponsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddCoupon(Coupon coupon)
        {
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();
            return "Coupon Added!!";
        }

        public async Task<string> DeleteCoupon(Coupon coupon)
        {
            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
            return "Coupon Removed!!";
        }

        public async Task<List<Coupon>> GetAllCoupons()
        {
            return await _context.Coupons.ToListAsync();
        }

        public async Task<Coupon> GetCoupon(Guid Id)
        {
            return await _context.Coupons.Where(x => x.CouponId == Id).FirstOrDefaultAsync();
        }

        public async Task<Coupon> GetCoupon(string code)
        {
            return await _context.Coupons.Where(x => x.CouponCode == code).FirstOrDefaultAsync();
        }

        public async Task<string> UpdateCoupon()
        {
            await _context.SaveChangesAsync();
            return "Coupon Updated !!";
        }
    }
}
