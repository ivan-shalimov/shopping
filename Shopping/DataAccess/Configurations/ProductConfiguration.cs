using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Models.Domain;

namespace Shopping.DataAccess.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(e => e.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.HasIndex(e => e.ProductKindId);
            builder.Property(e => e.ProductKindId).HasDefaultValue(ProductKind.DefaultProductKindId);
            builder.Property(c => c.Name).IsRequired();
        }
    }
}