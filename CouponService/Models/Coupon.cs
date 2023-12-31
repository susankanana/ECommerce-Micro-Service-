using System.ComponentModel.DataAnnotations;

namespace CouponService.Models
{
    public class Coupon
    {
        [Key]
        public Guid CouponId { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public int CouponAmount { get; set; }
        public int CouponMinAmount { get; set; }
    }
}
