using StoreManageAPI.ModelReturnData;
using StoreManageAPI.ViewModels.UserManager;

namespace StoreManageAPI.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ApiResponse> CreateUserAsync(CreateUserV model);
        public Task<ApiResponse> UpdateUserAsync(UpdateUserV model);
        public Task<ApiResponse> GetUserByIdAsync(string id);
        public Task<ApiResponse> LockUserAsync(string id);
        public Task<ApiResponse> UnLockUserAsync(string id);
        public Task<ApiResponse> SearchUsersAsync(string? searchKey, int? limit);
        public Task<ApiResponse> GetAllUserAsync(int? PageIndex, int? limit, int shop_id = 5);
        public Task<ApiResponse> GetAllLockUsersAsync(int? PageIndex, int? limit);
        public Task<ApiResponse> GetUserOfTreeAsync();
        public Task<ApiResponse> GetUserOfTreeByIdAsync(string userId);



    }
}
