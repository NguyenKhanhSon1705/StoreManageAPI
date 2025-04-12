using StoreManageAPI.ModelReturnData;
using StoreManageAPI.ViewModels.Shopes;

namespace StoreManageAPI.Services.Interfaces
{
    public interface ITablesService
    {
        public Task<ApiResponse> CreateTableAsync(CreateTablesV model);
        public Task<ApiResponse> UpdateTableAsync(CreateTablesV model);
        public Task<ApiResponse> GetListTableAsync(int idShop);
        public Task<ApiResponse> DeleteTableAsync(int idTable);
        public Task<ApiResponse> GetTableByIdAsync(int idTable);
        public Task<ApiResponse> ActiveTables(int idTable);
    }
}
