using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StoreManageAPI.Models
{
    public class DataStore : IdentityDbContext<User>
    {
        public DataStore(DbContextOptions<DataStore> options) : base(options)
        {
        }
        
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
