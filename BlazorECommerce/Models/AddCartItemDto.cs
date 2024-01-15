using System.ComponentModel.DataAnnotations;

namespace BlazorECommerce.Models
{
    public class AddCartItemDto
    {
        [Required]
        public Guid ProductId { get; set; }
        //public string ProductName { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
