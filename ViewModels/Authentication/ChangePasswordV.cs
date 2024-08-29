using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Authentication
{
    public class ChangePasswordV
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }
        [Required]
        [MaxLength(6)]
        public string? code { get; set; }

    }
}
