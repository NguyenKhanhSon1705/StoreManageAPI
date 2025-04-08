using StoreManageAPI.DTO.Dish;
using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Dish
{
    public class DishV
    {
        public int Id { get; set; }
  
        public int Shop_Id { get; set; }
        public int? selling_price_id { get; set; }
        public decimal? Selling_Price { get; set; }
        public List<PriceDishInfoDTO>? list_price { set; get; }
        public IList<ObMenuGroupV>? Arr_Menu_Group { get; set; }
        public string? Dish_Name { get; set; }
        public string? Unit_Name { get; set; }
        public decimal? Origin_Price { get; set; }
        public string? Image { get; set; }
        public int? Order { get; set; }
        public bool? Status { get; set; }
        public bool? Is_Hot { get; set; }
    }
}
