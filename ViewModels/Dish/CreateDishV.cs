using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Dish
{
    public class CreateDishV
    {
        public int Id { get; set; }
        public int Shop_Id { get; set; }
        public IList<int>? Arr_Menu_Group_Id { get; set; }
        public string? Dish_Name { get; set; }
        public string? Unit_Name { get; set; }
        public decimal? Origin_Price { get; set; }
        public decimal? Selling_Price { get; set; }
        public string? Image { get; set; }
        public int? Order { get; set; }
        public bool? Status { get; set; }
        public bool? Is_Hot { get; set; }
    }
}
