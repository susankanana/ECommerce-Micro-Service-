using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }= DateTime.Today;
        public double OrderTotal { get; set; }
        public List<OrderProduct> orderProducts { get; set; } = new List<OrderProduct>();
        public string? StripeSessionId { get; set; }

        public string Status { get; set; } = "Pending"; // is payment ongoing or was it successful

        public string PaymentIntent { get; set; } = string.Empty;
    }
}
