namespace StoreManageAPI.DTO.Dish
{
    public class PriceDishInfoDTO
    {
        public int? dish_id { set; get; }
        public int? price_id { set; get; }
        public bool? status { set; get; }
        public decimal? selling_price { set; get; } 
        public DateTime? create_at { set; get; }
    }
}
