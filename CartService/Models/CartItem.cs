using CartService.Models.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartService.Models
{
    public class CartItem
    {
        [Key]
        public Guid CartItemId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }=string.Empty;
        public int ProductPrice { get; set; }

        [NotMapped]
        public ProductDto? Product { get; set; }

        public int Quantity { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = default!;
        public Guid CartId { get; set; }
    }
}
