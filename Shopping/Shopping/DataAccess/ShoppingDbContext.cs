using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess.Configurations;
using Shopping.Models.Domain;

namespace Shopping.DataAccess
{
    public class ShoppingDbContext : DbContext
    {
        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<ReceiptItem> ReceiptItems { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductKind> ProductKinds { get; set; }

        public DbSet<Shop> Shops { get; set; }

        public ShoppingDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ReceiptConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptItemConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductKindConfiguration());
            modelBuilder.ApplyConfiguration(new ShopConfiguration());
        }
    }
}