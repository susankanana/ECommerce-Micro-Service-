using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlazorECommerce.Models
{
    public class CartItem
    {
        [Key]
        public Guid CartItemId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = default!;
        public Guid CartId { get; set; }
    }
}
