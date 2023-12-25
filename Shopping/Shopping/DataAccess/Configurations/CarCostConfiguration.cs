using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Models.Domain;

namespace Shopping.DataAccess.Configurations
{
    internal class CarCostConfiguration : IEntityTypeConfiguration<CarCost>
    {
        public void Configure(EntityTypeBuilder<CarCost> builder)
        {
            builder.ToTable("CarCosts");
            builder.HasKey(e => e.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Description).IsRequired();
            builder.Property(c => c.Price).IsRequired().HasPrecision(18, 2);
            builder.Property(c => c.Amount).IsRequired().HasPrecision(18, 3);
        }
    }
}