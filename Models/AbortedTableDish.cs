using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class AbortedTableDish
    {
        public int? aborted_table_id { get; set; }
        public int? dish_id { get; set; }
        public int? quantity { get; set; }
        public int? selling_price_id { get; set; }
        [ForeignKey(nameof(dish_id))]
        public Dish? dish { get; set; }
        [ForeignKey(nameof(aborted_table_id))]
        public AbortedTable? abortedTable { get; set; }
        [ForeignKey(nameof(selling_price_id))]
        public DishPriceVersion? dishPriceVersion { get; set; }
    }
}
