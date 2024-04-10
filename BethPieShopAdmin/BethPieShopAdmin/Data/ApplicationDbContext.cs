using BethPieShopAdmin.Models;
using Microsoft.EntityFrameworkCore;

namespace BethPieShopAdmin.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {
        }

        public DbSet<Category> categories { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<OrderDetail> orderDetails { get; set; }

        public DbSet<Pie> pies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
