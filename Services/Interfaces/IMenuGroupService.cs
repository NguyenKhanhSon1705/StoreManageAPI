using StoreManageAPI.ModelReturnData;
using StoreManageAPI.ViewModels.Dish;

namespace StoreManageAPI.Services.Interfaces
{
    public interface IMenuGroupService
    {
        public Task<ApiResponse> CreateGroupMenuAsync(CreateMenuGroupV model);
        public Task<ApiResponse> UpdateGroupMenuAsync(CreateMenuGroupV model);
        public Task<ApiResponse> GetAllNameMenuGroup(int shopId);
        public Task<ApiResponse> DeleteGroupMenuAsync(int id);
        public Task<ApiResponse> GetAllGroupMenusAsync(int shopId, int _pageIndex = 1, int limit = 10, string search = "");
    }
}
