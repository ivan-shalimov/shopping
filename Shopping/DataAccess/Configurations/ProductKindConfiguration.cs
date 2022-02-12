using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Models.Domain;

namespace Shopping.DataAccess.Configurations
{
    internal class ProductKindConfiguration : IEntityTypeConfiguration<ProductKind>
    {
        public void Configure(EntityTypeBuilder<ProductKind> builder)
        {
            builder.ToTable("ProductKinds");
            builder.HasKey(e => e.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Name).IsRequired();
        }
    }
}