using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enum to string conversion
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            // Configure OrderItem composite keys if necessary
        }
    }
}
