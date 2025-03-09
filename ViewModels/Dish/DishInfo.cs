using StoreManageAPI.DTO.Dish;

namespace StoreManageAPI.ViewModels.Dish
{
    public class DishInfo
    {
        public int Id { get; set; } 
        public string? Dish_Name { get; set; }
        public string? Unit_Name { get; set; }
        public decimal? Selling_Price { get; set; }
        public int? selling_price_id { get; set; }
        public string? Image { get; set; }
        public int? Inventory {  get; set; }

    }
}
