using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Shopes
{
    public class GetShopV
    {
        public int Id { get; set; }
        [Required]
        public string? ShopName { get; set; }
        [Phone]
        public string? ShopPhone { get; set; }
        public string? ShopLogo { get; set; }
        public string? ShopAddress { get; set; }
        public bool? IsActive { get; set; }
    }
}
