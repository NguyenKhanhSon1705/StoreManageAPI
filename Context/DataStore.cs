using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Models;
using System.Reflection.Emit;

namespace StoreManageAPI.Context
{
    public class DataStore : IdentityDbContext<User>
    {
        public DataStore(DbContextOptions<DataStore> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName != null && tableName.StartsWith("AspNet") )
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }


            builder.Entity<ShopUser>()
                .HasKey(key => new { key.UserId, key.ShopId });
            
        }   
        // các bảng liên quan đến người dùng
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        

        // Các bảng liên quan đến của hàng
        public DbSet<Shop> Shop { get; set; }
        public DbSet<ShopUser> ShopUser{ get; set; }
        public DbSet<Areas> Areas { get; set; }
        public DbSet<Tables> Tables { get; set; }
    }
}
