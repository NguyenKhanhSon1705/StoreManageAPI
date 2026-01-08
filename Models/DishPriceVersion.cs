using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class DishPriceVersion
    {
        [Key]
        public int id { get; set; }
        public int? dish_id  { get; set; }
        public string? price_version { get; set; }
        public decimal? selling_price {  get; set; }
        public DateTime? create_at {  get; set; }
        public DateTime? update_at { get;set; }
        public bool? status {  get; set; }

        [ForeignKey(nameof(dish_id))]
        public virtual Dish? dish { get; set; }
    }
}
