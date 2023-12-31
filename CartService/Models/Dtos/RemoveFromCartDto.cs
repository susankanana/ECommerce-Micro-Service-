using System.ComponentModel.DataAnnotations;

namespace CartService.Models.Dtos
{
    public class RemoveFromCartDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int ProductQuantity { get; set; }
    }
}
