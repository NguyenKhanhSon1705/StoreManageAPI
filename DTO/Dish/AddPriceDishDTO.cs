namespace StoreManageAPI.DTO.Dish
{
    public class AddPriceDishDTO
    {
        public int? dish_id { get; set; }
        public int? price_id { get; set; }
        public decimal? new_price { get; set; }
    }
}
