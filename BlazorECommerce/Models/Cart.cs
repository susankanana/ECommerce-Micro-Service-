using System.ComponentModel.DataAnnotations;

namespace BlazorECommerce.Models
{
    public class Cart
    {
        [Key]
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public double CouponDiscount { get; set; }
        public double CartTotal { get; set; }
        public string CouponCode { get; set; } = string.Empty;
    }
}
