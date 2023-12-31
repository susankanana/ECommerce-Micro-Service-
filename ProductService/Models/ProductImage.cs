using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Models
{
    public class ProductImage
    {
        [Key]
        public Guid ImageId { get; set; }

        public string Image { get; set; } = string.Empty;

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = default!;

        public Guid ProductId { get; set; }
    }
}
