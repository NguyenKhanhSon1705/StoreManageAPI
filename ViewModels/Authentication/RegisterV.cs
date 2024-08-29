using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Authentication
{
    public class RegisterV
    {
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; } = "";
        [Required]
        [MaxLength(15)]
        [Phone]
        public string? Phone { get; set; } = "";
        [Required]
        [MaxLength(50)]
        public string? Password { get; set; } = "";

    }
}
