﻿namespace CartService.Models.Dtos
{
    public class CartAndCartItemsResponseDto
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public List<CartItemResponseDto> Items { get; set; } = new List<CartItemResponseDto>();
        public double CouponDiscount { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public double CartTotal { get; set; }
    }
}
