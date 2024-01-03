using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static Azure.Core.HttpHeader;
using OrderService.Models;

namespace OrderService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
    }
}
   
