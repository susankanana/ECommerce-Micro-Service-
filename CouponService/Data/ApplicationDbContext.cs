using Microsoft.EntityFrameworkCore;
using CouponService.Models;
using System.Collections.Generic;
using static Azure.Core.HttpHeader;

namespace CouponService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Coupon> Coupons { get; set; }
    }
}
   
