using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class BillDetails
    {
        public int? bill_id { set; get; }
        public int? dish_id { set; get; }
        public int? selling_price_id { set;get; }
        public int? quantity { set; get; }
        public string? notes { set; get; }

        [ForeignKey(nameof(bill_id))]
        public Bill? Bill { set; get; }
        [ForeignKey(nameof(dish_id))]
        public Dish? Dish { set; get; }
        [ForeignKey(nameof(selling_price_id))]
        public DishPriceVersion? DishPriceVersion { set; get; }

    }
}
