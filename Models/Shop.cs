using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.Models
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? ShopName { get; set; }
        [Phone]
        public string? ShopPhone { get; set; }
        public string? ShopLogo { get; set; }
        public string? ShopAddress { get; set; }
        public bool? IsActive { get; set; }
        public bool? LockStore { get; set; } = false;
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual ICollection<ShopUser>? StoreUsers { get; set; }
    }
}
