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

        public DbSet<CarCost> CarCosts { get; set; }

        public DbSet<PriceChangeProjection> PriceChangeProjections { get; set; }

        public DbSet<Tariff> Tariffs { get; set; }

        public DbSet<TariffPeriod> TariffPeriods { get; set; }

        public DbSet<Bill> Bills { get; set; }

        public DbSet<BillItem> BillItems { get; set; }


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
            modelBuilder.ApplyConfiguration(new CarCostConfiguration());
            modelBuilder.ApplyConfiguration(new PriceChangeProjectionConfiguration());

            modelBuilder.ApplyConfiguration(new TariffConfiguration());
            modelBuilder.ApplyConfiguration(new TariffPeriodConfiguration());
            modelBuilder.ApplyConfiguration(new BillConfiguration());
            modelBuilder.ApplyConfiguration(new BillItemConfiguration());
        }
    }
}