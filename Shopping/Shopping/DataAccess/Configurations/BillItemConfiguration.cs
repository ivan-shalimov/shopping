using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Models.Domain;

namespace Shopping.DataAccess.Configurations
{
    internal class BillItemConfiguration : IEntityTypeConfiguration<BillItem>
    {
        public void Configure(EntityTypeBuilder<BillItem> builder)
        {
            builder.ToTable("BillItems");
            builder.HasKey(e => e.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.HasIndex(e => e.TariffId);
            builder.HasIndex(e => e.BillId);

            builder.Property(c => c.Rate).IsRequired().HasPrecision(18, 5);
        }
    }
}