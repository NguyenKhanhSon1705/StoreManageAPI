namespace StoreManageAPI.DTO.Reports
{
    public class ReportBillReturnDTO
    {
        public int? id { get; set; }
        public string? bill_code { get; set; }
        public string? user_name { get; set; }
        public string? table_name { get; set; }
        public string? area_name { get; set; }
        public string? paymentMethod { get; set; }
        public decimal? VAT { get;set; }
        public decimal? discount { get; set; }
        public decimal? total_money { get; set; }
        public int? total_quantity { get; set; }
        public DateTime? time_start { get; set; }
        public DateTime? time_end { get; set; }
    }
}
