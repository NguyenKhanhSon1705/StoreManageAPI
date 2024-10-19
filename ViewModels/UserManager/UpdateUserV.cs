using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.UserManager
{
    public class UpdateUserV
    {
        public string? Id { get; set; }
        
        public string? FullName { get; set; }
        
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public IFormFile? Picture { get; set; }
        public DateTime? BirthDay { get; set; }
        public int? Gender { get; set; }
    }
}
