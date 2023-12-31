using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models.Dtos
{
    public class CartItemDto
    {
        public Guid CartItemId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("CartId")]
        public CartDto cartDto { get; set; } = default!;
        public Guid CartId { get; set; }
    }
}
