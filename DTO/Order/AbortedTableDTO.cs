namespace StoreManageAPI.DTO.Order
{
    public class AbortedTableDTO
    {
        public int? shop_id { set; get; }
        public int? table_Id { set; get; }
        public string? reason_abort { set; get; }
        public decimal? total_money { set; get; }
        public int? total_quantity { set; get; }   
    }
}
