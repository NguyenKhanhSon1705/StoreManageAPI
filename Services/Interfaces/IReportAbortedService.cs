using StoreManageAPI.DTO.Reports;
using StoreManageAPI.ModelReturnData;

namespace StoreManageAPI.Services.Interfaces
{
    public interface IReportAbortedService
    {
        public Task<ApiResponse> GetAllReportAbortedAsync(ReportFilterDTO model);
        public Task<ApiResponse> ShowAborteDetailAsync(int id);
    }
}
