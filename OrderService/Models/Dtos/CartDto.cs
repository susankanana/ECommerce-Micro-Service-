namespace OrderService.Models.Dtos
{
    public class CartDto
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public double CouponDiscount { get; set; }
        public double CartTotal { get; set; }
        public string CouponCode { get; set; } = string.Empty;
    }
}
