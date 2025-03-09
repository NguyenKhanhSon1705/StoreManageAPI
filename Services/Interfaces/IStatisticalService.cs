using StoreManageAPI.DTO.Reports;
using StoreManageAPI.ModelReturnData;

namespace StoreManageAPI.Services.Interfaces
{
    public interface IStatisticalService
    {
        public Task<ApiResponse> RevenueReportAsync(FilterRevenueReportDTO model);
    }
}
