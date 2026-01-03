namespace StoreManageAPI.DTO.Reports
{
    public class DishBillInfo
    {
        public int? dish_id { get; set; }
        public string? dish_name { get; set; }
        public string? image { get; set; }
        public int? quantity { get; set; }
        public string? notes { get; set; }
        public decimal? price { get; set; }
    }
    public class ReportBillDetailReturnDTO
    {
        public int? bill_id { get; set; }
        public List<DishBillInfo>? list_item { get; set; }
        public string? transaction_status { get; set; }
        public string? payment_method { get; set; }
        public string? description { get; set; }
        public string? bank_code { get; set; }
        public DateTime? payment_date { get; set; }
    }
}
