namespace StoreManageAPI.DTO.Reports
{
    public class ReportFilterDTO
    {
        // ID cửa hàng
        public int shop_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string? search_bill_code { get; set; }
        public string? employee_id { get; set; }
        public int page_index { get; set; } = 1;
        public int limit { get; set; } = 10;
    }
}
