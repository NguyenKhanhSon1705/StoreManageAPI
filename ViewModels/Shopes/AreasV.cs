using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Shopes
{
    public class AreasV
    {
        public int Id { get; set; }
        [Required]
        public string? AreaName { get; set; }
        public int? IdShop { get; set; }
    }
}
