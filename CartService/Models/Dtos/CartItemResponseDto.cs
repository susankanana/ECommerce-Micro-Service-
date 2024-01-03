namespace CartService.Models.Dtos
{
    public class CartItemResponseDto
    {
        public string ProductName { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid CartItemId { get; set; }
        public int ProductPrice { get; set; }
    }
}
