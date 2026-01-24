using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Models.Domain;

namespace Shopping.DataAccess.Configurations
{
    internal class TariffPeriodConfiguration : IEntityTypeConfiguration<TariffPeriod>
    {
        public void Configure(EntityTypeBuilder<TariffPeriod> builder)
        {
            builder.ToTable("TariffPeriods");
            builder.HasKey(e => e.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.StartOn).IsRequired();
            builder.Property(c => c.Rate).IsRequired().HasPrecision(18, 5);

            builder.HasOne(tp => tp.Tariff)
                   .WithMany(t => t.TariffPeriods)
                   .HasForeignKey(tp => tp.TariffId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}