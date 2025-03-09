namespace StoreManageAPI.DTO.Dish
{
    public class ImportDishDTO
    {
        public int? id { get; set; }
        public string? dish_name { get; set; }  
        public string? image { get; set; }  
        public string? menu_group { get; set; }
        public string? selling_price { get; set; }
        public string? selling_price_old { get; set; } 
        public string? origin_price { get; set; }
        public string? unit_name { get; set; }
        public string? order { get; set; }  
        public bool? is_hot { get; set; }
        public bool? status { get; set; }

    }
}
