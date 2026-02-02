//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Content_App.Infrastructure.Configurations
//{
//    public class ApproveConfig : IEntityTypeConfiguration<Approve>
//    {
//        public void Configure(EntityTypeBuilder<Approve> builder)
//        {
//            builder.ToTable("approve");

//            builder.HasKey(x => x.Id);

//            builder.Property(x => x.Id)
//                   .HasColumnName("id");

//            builder.Property(x => x.ItemId)
//                   .HasColumnName("item_id")
//                   .IsRequired();

//            builder.Property(x => x.RequestorId)
//                   .HasColumnName("requestor_id")
//                   .IsRequired();

//            builder.Property(x => x.Qty)
//                   .HasColumnName("qty")
//                   .IsRequired();

//            builder.Property(x => x.Kind)
//                   .HasColumnName("kind")
//                   .IsRequired();

//            builder.Property(x => x.Reason)
//                   .HasColumnName("reason")
//                   .IsRequired();

//            builder.Property(x => x.RequestTime)
//                   .HasColumnName("request_time")
//                   .HasDefaultValueSql("now()");

//            builder.Property(x => x.Status)
//                   .HasColumnName("status")
//                   .HasDefaultValue("pending");

//            builder.HasOne(x => x.Item)
//                   .WithMany()
//                   .HasForeignKey(x => x.ItemId)
//                   .HasConstraintName("fk_approve_item")
//                   .OnDelete(DeleteBehavior.Restrict);

//            builder.HasOne(x => x.Requestor)
//                   .WithMany(a => a.Approves)
//                   .HasForeignKey(x => x.RequestorId)
//                   .HasConstraintName("fk_approve_requestor")
//                   .OnDelete(DeleteBehavior.Restrict);
//        }
//    }
//}
