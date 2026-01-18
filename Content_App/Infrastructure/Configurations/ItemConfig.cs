using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Content_App.Infrastructure.Configurations
{
    public class ItemConfig : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("item");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("id");

            builder.Property(x => x.ItemCode)
                   .HasColumnName("item_code")
                   .IsRequired();

            builder.Property(x => x.EnName)
                   .HasColumnName("en_name");

            builder.Property(x => x.VnName)
                   .HasColumnName("vn_name");

            builder.Property(x => x.Quantity)
                   .HasColumnName("quantity")
                   .HasDefaultValue(0);

            builder.Property(x => x.Unit)
                   .HasColumnName("unit");

            builder.Property(x => x.Cost)
                   .HasColumnName("cost")
                   .HasColumnType("numeric(15,3)");

            builder.Property(x => x.Currency)
                   .HasColumnName("currency");

            builder.Property(x => x.Maker)
                   .HasColumnName("maker");

            builder.Property(x => x.Supplier)
                   .HasColumnName("supplier");

            builder.Property(x => x.Image)
                   .HasColumnName("image");

            builder.Property(x => x.DeptId)
                   .HasColumnName("dept_id")
                   .IsRequired();

            builder.Property(x => x.AreaId)
                   .HasColumnName("area_id")
                   .IsRequired();

            builder.HasOne(x => x.Department)
                   .WithMany()
                   .HasForeignKey(x => x.DeptId)
                   .HasConstraintName("fk_item_dep")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Area)
                   .WithMany()
                   .HasForeignKey(x => x.AreaId)
                   .HasConstraintName("fk_item_area")
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
