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
            builder.Entity<MenuGroupDish>()
                .HasKey(key => new { key.Dish_Id, key.Menu_Group_Id });
            builder.Entity<TableDishs>()
                .HasKey(key => new { key.dishId, key.tableId });
            builder.Entity<AbortedTableDish>()
                .HasKey(key => new {key.dish_id, key.aborted_table_id});
            builder.Entity<BillDetails>()
                .HasKey(key=> new {key.dish_id , key.bill_id});

            builder.Entity<DishPriceVersion>().HasKey(x => x.id);
            
        }   
        // các bảng liên quan đến người dùng
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        

        // Các bảng liên quan đến của hàng
        public DbSet<Shop> Shop { get; set; }
        public DbSet<ShopUser> ShopUser{ get; set; }
        public DbSet<Areas> Areas { get; set; }
        public DbSet<Tables> Tables { get; set; }

        // Các bảng liên quan đến món ăn

        public DbSet<MenuGroup> MenuGroups { get; set; }
        public DbSet<MenuGroupDish> Menu_Groups_Dish { get; set; }
        public DbSet<Dish> Dish { get; set; }
        public DbSet<DishPriceVersion> DishPriceVersions { get; set; }

        // table dish
        public DbSet<TableDishs> TableDishs { get; set; }
        public DbSet<AbortedTable> AbortedTables { get; set; }
        public DbSet<AbortedTableDish> AbortedTablesDish { get; set; }

        // thanh toán

        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetails> BillDetails { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
    }
}
