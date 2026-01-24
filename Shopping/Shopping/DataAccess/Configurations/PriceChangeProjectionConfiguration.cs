using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Models.Domain;

namespace Shopping.DataAccess.Configurations
{
    internal class PriceChangeProjectionConfiguration : IEntityTypeConfiguration<PriceChangeProjection>
    {
        public void Configure(EntityTypeBuilder<PriceChangeProjection> builder)
        {
            builder.ToTable("PriceChangeProjections");
            builder.HasKey(e => e.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Version).IsRowVersion();

            builder.Property(e => e.ChangedDate);

            builder.HasIndex(e => e.ProductId);
            builder.HasIndex(e => e.Shop);
            builder.HasIndex(e => new { e.ProductId, e.Shop }).IsUnique();

            builder.Property(c => c.PreviousPrice).IsRequired().HasPrecision(18, 2);
            builder.Property(c => c.LastPrice).IsRequired().HasPrecision(18, 2);
            builder.Property(c => c.ChangePercent).IsRequired().HasPrecision(18, 2);
        }
    }
}