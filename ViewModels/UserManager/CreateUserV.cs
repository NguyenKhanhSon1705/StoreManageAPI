using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.UserManager
{
    public class CreateUserV
    {
        public string? Id { get; set; }
        [Required]
        public int? IdShop { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDay { get; set; }
        public int? Gender { get; set; }
        public string? RoleId { get; set; }

    }
}
