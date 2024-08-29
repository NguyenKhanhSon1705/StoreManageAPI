using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Authentication
{
    public class LogInV
    {
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }
    }
}
