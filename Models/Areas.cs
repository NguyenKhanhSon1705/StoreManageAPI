using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class Areas
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? AreaName { get; set; } 

        public int? ShopId { get; set; }
        [ForeignKey("ShopId")]
        public virtual Shop? Shop { get; set; }
    }
}
