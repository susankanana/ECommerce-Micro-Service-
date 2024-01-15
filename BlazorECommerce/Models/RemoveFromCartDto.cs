using System.ComponentModel.DataAnnotations;

namespace BlazorECommerce.Models
{
    public class RemoveFromCartDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int ProductQuantity { get; set; }
    }
}
