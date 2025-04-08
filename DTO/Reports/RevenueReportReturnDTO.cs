using StoreManageAPI.ViewModels.Ordertables;

namespace StoreManageAPI.DTO.Reports
{
    public class RevenueReportReturnDTO
    {
        public decimal? total_nuvenue { set; get; }
        public decimal? total_transaction_online { get; set; }
        public decimal? pendding_nuvenue { set; get; }

        public int? total_billed { set; get; }
        public int? pendding_bill { set; get; }

        public decimal? total_aborted_money { get; set; }
        public int? total_aborted { set; get; }
        public int? total_aborted_dish { get; set; }

        public List<DishInfoV>? list_dish_hot { get; set; }
        public List<DishInfoV>? list_dish_not_hot { get; set; }

    }
}
