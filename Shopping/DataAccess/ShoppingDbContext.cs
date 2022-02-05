using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess.Configurations;
using Shopping.Models.Domain;

namespace Shopping.DataAccess
{
    public class ShoppingDbContext : DbContext
    {
        public DbSet<Purchase> Purchases { get; set; }

        public ShoppingDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PurchaseConfiguration());
        }
    }
}