using System.ComponentModel.DataAnnotations;

namespace RewardsService.Models
{
    public class Reward
    {
        [Key]
        public Guid RewardId { get; set; }
        public Guid OrderId { get; set; }

        public double OrderTotal { get; set; }

        public double Points { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
