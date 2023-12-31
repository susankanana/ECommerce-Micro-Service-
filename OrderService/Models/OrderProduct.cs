using OrderService.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class OrderProduct
    {
        [Key]
        public Guid OrderProductId { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public Guid CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
