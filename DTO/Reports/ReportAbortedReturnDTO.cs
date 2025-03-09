namespace StoreManageAPI.DTO.Reports
{
    public class ReportAbortedReturnDTO
    {
        public int? id { get; set; }
        public string? user_name { get; set; }
        public string? table_name { get; set; }
        public string? area_name { get; set; }
        public string? reason_abort { get; set; }
        public decimal? total_money { get; set; }
        public int? total_quantity { get; set; }
        public DateTime? created_table_at { get; set; }
        public DateTime? aborted_date { get; set; }

    }
}
