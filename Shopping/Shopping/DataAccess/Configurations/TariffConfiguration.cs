using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Models.Domain;

namespace Shopping.DataAccess.Configurations
{
    internal class TariffConfiguration : IEntityTypeConfiguration<Tariff>
    {
        public void Configure(EntityTypeBuilder<Tariff> builder)
        {
            builder.ToTable("Tariffs");
            builder.HasKey(e => e.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Description).IsRequired();
            builder.Property(c => c.GroupName).IsRequired();
            builder.Property(c => c.RateType).IsRequired();
            builder.HasMany(e=>e.TariffPeriods)
                .WithOne()
                .HasForeignKey(tp=>tp.TariffId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}