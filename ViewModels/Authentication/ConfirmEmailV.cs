using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Authentication
{
    public class ConfirmEmailV
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        [MaxLength(6)]
        public string Code { get; set; } = "";
    }
}
