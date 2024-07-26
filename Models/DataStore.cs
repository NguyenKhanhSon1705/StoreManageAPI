using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StoreManageAPI.Models
{
    public class DataStore : IdentityDbContext
    {
        public DataStore(DbContextOptions options) : base(options)
        {
        }
    }
}
