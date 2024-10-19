using StoreManageAPI.ModelReturnData;
using StoreManageAPI.ViewModels.Shopes;
using StoreManageAPI.ViewModels.Stores;

namespace StoreManageAPI.Services.Interfaces
{
    public interface IAreasService
    {
        public Task<ApiResponse> CreateAreaAsync(AreasV model);
        public Task<ApiResponse> UpdateAreaAsync(AreasV model );
        //public Task<ApiResponse> IsActiveAreaAsync(int id);
        public Task<ApiResponse> GetListAreaAsync(int idShop);
        public Task<ApiResponse> DeleteAreaAsync(int id);
        public Task<ApiResponse> GetAreaByIdAsync(int id);
    }
}
