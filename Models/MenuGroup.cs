using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class MenuGroup
    {
        [Key]
        public int Id { get; set; }
        [Required]        
        
        public int ShopId { get; set; }
        [ForeignKey(nameof(ShopId))]
        public Shop? Shop { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; } = "";
        public string? Image { get; set; } = "";
        public int? Order { get; set; }
        public bool Status { get; set; } = true;
    }
}
