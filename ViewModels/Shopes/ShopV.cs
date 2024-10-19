using StoreManageAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Stores
{
    public class ShopV
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? ShopName { get; set; }
        [Phone]
        [Required]
        public string? ShopPhone { get; set; }
        public IFormFile? ShopLogo { get; set; } = null;
        public string? ShopAddress { get; set; }
        public bool? IsActive { get; set; }
        public bool? LockStore { get; set; } = false;
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
