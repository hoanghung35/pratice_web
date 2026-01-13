using Content_App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Content_App.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        
    }
}
