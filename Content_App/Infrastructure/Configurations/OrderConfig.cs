//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Content_App.Infrastructure.Configurations
//{
//    public class OrderConfig : IEntityTypeConfiguration<OrderItem>
//    {
//        public void Configure(EntityTypeBuilder<OrderItem> builder)
//        {
//            builder.ToTable("order_item");

//            builder.HasKey(x => x.Id);

//            builder.Property(x => x.Id)
//                   .HasColumnName("id");

//            builder.Property(x => x.ItemId)
//                   .HasColumnName("item_id")
//                   .IsRequired();

//            builder.Property(x => x.PicId)
//                   .HasColumnName("pic_id")
//                   .IsRequired();

//            builder.Property(x => x.Qty)
//                   .HasColumnName("qty")
//                   .IsRequired();

//            builder.Property(x => x.PlanOrder)
//                   .HasColumnName("plan_order")
//                   .HasColumnType("date")
//                   .IsRequired();

//            builder.Property(x => x.Reason)
//                   .HasColumnName("reason")
//                   .IsRequired();

//            builder.Property(x => x.DateModify)
//                   .HasColumnName("date_modify")
//                   .HasDefaultValueSql("now()");

//            builder.HasOne(x => x.Item)
//                   .WithMany()
//                   .HasForeignKey(x => x.ItemId)
//                   .HasConstraintName("fk_order_item")
//                   .OnDelete(DeleteBehavior.Restrict);

//            builder.HasOne(x => x.Pic)
//                   .WithMany(a => a.OrderItems)
//                   .HasForeignKey(x => x.PicId)
//                   .HasConstraintName("fk_order_pic")
//                   .OnDelete(DeleteBehavior.Restrict);
//        }
//    }
//}
