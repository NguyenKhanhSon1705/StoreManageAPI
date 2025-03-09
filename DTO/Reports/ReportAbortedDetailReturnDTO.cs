namespace StoreManageAPI.DTO.Reports
{
    public class ReportAbortedDetailReturnDTO
    {
        public int? aborted_id { get; set; }
        public int? dish_id { get; set; }
        public string? dish_name { get; set; }
        public string? image { get; set; }
        public int? quantity { get; set; }
        public decimal? price { get; set; }

    }
}
