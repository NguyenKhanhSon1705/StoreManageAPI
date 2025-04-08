using StoreManageAPI.DTO.Reports;
using StoreManageAPI.ModelReturnData;

namespace StoreManageAPI.Services.Interfaces
{
    public interface IReportBillService
    {
        public Task<ApiResponse> GetAllReportBillAsync(ReportFilterDTO model);
        public Task<ApiResponse> ShowBillDetailAsync(int id);
    }
}
