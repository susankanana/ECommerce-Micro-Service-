namespace CartService.Models.Dtos
{
    public class CouponDto
    {
        public Guid CouponId { get; set; }

        public string CouponCode { get; set; } = string.Empty;
        public int CouponAmount { get; set; }
        public int CouponMinAmount { get; set; }
    }
}
