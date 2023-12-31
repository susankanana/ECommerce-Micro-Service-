using RewardsService.Models;
using RewardsService.Services.IServices;
using RewardsService.Data;
using Microsoft.EntityFrameworkCore;

namespace RewardsService.Services
{
    public class RewardService : IReward
    {
        private DbContextOptions<ApplicationDbContext> options;

        public RewardService(DbContextOptions<ApplicationDbContext> options)
        {
            this.options = options;
        }

        public async Task AddReward(Reward reward)
        {
            var _context = new ApplicationDbContext(options);
            _context.Rewards.Add(reward);
            await _context.SaveChangesAsync();
        }
    }
}
