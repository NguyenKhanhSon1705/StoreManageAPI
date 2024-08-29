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
        public string? Gender { get; set; }
        public int IsOwner { get; set; } = 0;
        public string? VerifiCode { get; set; }
        public DateTime? CodeExpireTime { get; set; }
        public int? LookAccount { get; set; } = 0;
    }
}
