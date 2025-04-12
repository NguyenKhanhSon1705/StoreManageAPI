using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class ShopUser
    {
        public string? UserId { get; set; }

        public int ShopId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        [ForeignKey("ShopId")]            
        public virtual Shop? Shop { get; set; }
    }
}
