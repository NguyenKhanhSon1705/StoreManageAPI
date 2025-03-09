using StoreManageAPI.ModelReturnData;

namespace StoreManageAPI.Services.Interfaces
{
    public interface ITableAreaService
    {
        public Task<ApiResponse> GetTableByArea(int areaId);
    }
}
