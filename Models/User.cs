using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class User : IdentityUser
    {
        public string? ManagerID { get; set; }
        [ForeignKey(nameof(ManagerID))]
        public User? _User { get; set; }

        public string? FullName { get; set; }
        public string? Address { get; set; }

        public DateTime? CreationDate { get; set; }
        public string? Picture { get; set; } 
        public DateTime? BirthDay { get; set; }
        public int? Gender { get; set; }
        public int IsOwner { get; set; } = 0;
        public string? VerifiCode { get; set; }
        public DateTime? CodeExpireTime { get; set; }
        public DateTime? LockAtDate { get; set; } = null;
        public string? LockByUser { get; set; } = null;
        public bool? IsLock { get; set; } = null;
        public virtual ICollection<ShopUser>? StoreUsers { get; set; }

    }
}
