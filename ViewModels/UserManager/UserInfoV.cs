using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.UserManager
{
    public class UserInfoV
    {
        public string? Id { get; set; } 
        public string? ManagerID { get; set; }
        public string? ManagerName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Gender { get; set; }
        public DateTime? LockAtDate { get; set; } = null;
        public string? LockByUser { get; set; } = null;
        public bool? IsLock { get; set; } = null;
        public IList<string>? Roles { get; set; }
    }
}
