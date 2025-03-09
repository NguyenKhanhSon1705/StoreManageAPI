using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class AbortedTable
    {
        [Key]
        public int? id { get; set; }
        public string? user_id { get; set; }
        public int? table_id { get; set; }
        public int? shop_id { get; set; }
        public string? reason_abort { get; set; }
        public decimal? total_moneny { get; set; }
        public int? total_quantity_dish { get; set; }
        public DateTime? create_table_date { get; set; }
        public DateTime? aborted_date { get; set; }

        [ForeignKey(nameof(user_id))]
        public virtual User? user { get; set; }
        [ForeignKey(nameof(table_id))]
        public virtual Tables? Table { get; set; }
        [ForeignKey(nameof(shop_id))]
        public virtual Shop? Shop { get; set; }
    }
}
