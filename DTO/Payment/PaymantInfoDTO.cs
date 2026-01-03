using StoreManageAPI.ViewModels.Ordertables;

namespace StoreManageAPI.DTO.Payment
{
    public class PaymantInfoDTO
    {
        public int? bill_number {  get; set; }
        public string? table_name { set; get; }
        public string? area_name { set; get; }
        public string? shop_name { set; get; }
        public string? hotline { set; get; }
        public string? staff_name { set; get; }
        public string? address_shop {  set; get; }
        public decimal? total_money { set; get; }
        public int? total_quantity { set; get; }
        public string? payment_method { set; get; }
        public DateTime? time_start { set; get; }
        public DateTime? time_end { set; get; }
        public List<DishInfoV>? listDish { set; get; }
    }
}
