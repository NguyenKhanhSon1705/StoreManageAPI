using StoreManageAPI.ModelReturnData;
using StoreManageAPI.ViewModels.Stores;

namespace StoreManageAPI.Services.Interfaces
{
    public interface IShopSerVice
    {
        public Task<ApiResponse> CreateShopAsync(ShopV model);
        public Task<ApiResponse> UpdateShopAsync(ShopV model);
        public Task<ApiResponse> IsActiveShopAsync(int id);
        public Task<ApiResponse> GetListShopAsync();
        public Task<ApiResponse> DeleteShopAsync(int id, string password);
        public Task<ApiResponse> GetShopByIdAsync(int id);
    }
}
