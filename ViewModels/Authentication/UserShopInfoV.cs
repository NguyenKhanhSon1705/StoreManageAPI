using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Authentication
{
    public class UserShopInfoV
    {
        public int shopId { get; set; }

        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Picture {  get; set; }
        public string? ShopName { get; set; }
        public string? ShopLogo { get; set; } = null;
        public bool? IsActive { get; set; }

    }
}
