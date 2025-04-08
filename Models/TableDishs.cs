using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class TableDishs
    {
        public int? tableId { get; set; } 
        public int? dishId { get; set; }
        [ForeignKey("dishId")]
        public virtual Dish? dish { get; set; }
        [ForeignKey("tableId")]
        public virtual Tables? table { get; set; }
        public int? quantity {  get; set; }
        public string? notes { get; set; }
        public decimal? selling_Price { get; set; }
    }
}
