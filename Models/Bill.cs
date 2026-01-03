using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class Bill
    {
        [Key]
        public int? id { set; get; }
        public string? bill_code { set; get; }
        public string? user_id { set; get; }
        public int? table_id { set; get; }
        public int? shop_id { set; get; }
        public DateTime? time_start { set; get; }
        public DateTime? time_end { set; get; }
        public decimal? total_money { set; get; }
        public int? total_quantity { set; get; }
        public decimal? VAT { set; get; }
        public decimal? discount { set; get; }
        public int? payment_method { set; get; }

        [ForeignKey(nameof(user_id))]
        public User? User { get; set; }
        [ForeignKey(nameof(table_id))]
        public Tables? Tables { get; set; }
        [ForeignKey(nameof(shop_id))]
        public Shop? Shop { get; set; }
    }
}
