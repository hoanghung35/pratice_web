using Content_App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Content_App.Infrastructure.Configurations
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("account");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserCode).HasColumnName("user_code");
            builder.Property(x => x.Password).HasColumnName("password");

            builder.HasOne(x => x.Role)
                   .WithMany(r => r.Accounts)
                   .HasForeignKey(x => x.RoleId);
        }
    }
}
