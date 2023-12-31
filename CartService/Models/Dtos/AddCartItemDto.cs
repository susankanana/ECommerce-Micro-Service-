using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CartService.Models.Dtos
{
    public class AddCartItemDto
    {
        [Required]
        public Guid ProductId { get; set; }
        //public string ProductName { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public int Quantity { get; set; } = 1;
        //public Guid CartId { get; set; }
    }
}
