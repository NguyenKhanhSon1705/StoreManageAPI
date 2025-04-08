using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Dish_Name { get; set; }
        [Required]
        public string? Unit_Name { get; set; }
        public decimal? Origin_Price { get; set; }
        
        public string? Image { get; set; }
        public int? Order {  get; set; }
        public bool? Status { get; set; }
        public DateTime? Create_At { get; set; }
        public bool? Is_Hot { get; set; }
        public int? Inventory {  get; set; }

        public virtual DishPriceVersion? Price { get; set; }
    }
}
