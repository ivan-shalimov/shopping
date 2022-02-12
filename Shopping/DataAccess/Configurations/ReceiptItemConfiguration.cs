using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Models.Domain;

namespace Shopping.DataAccess.Configurations
{
    internal class ReceiptItemConfiguration : IEntityTypeConfiguration<ReceiptItem>
    {
        public void Configure(EntityTypeBuilder<ReceiptItem> builder)
        {
            builder.ToTable("ReceiptItems");
            builder.HasKey(e => e.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.HasIndex(e => e.ProductId);
            builder.HasIndex(e => e.ReceiptId);
            builder.Property(c => c.Price).IsRequired().HasPrecision(18, 2);
            builder.Property(c => c.Amount).IsRequired().HasPrecision(18, 3);
        }
    }
}