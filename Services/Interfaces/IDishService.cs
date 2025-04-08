using StoreManageAPI.DTO.Dish;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.ViewModels.Dish;

namespace StoreManageAPI.Services.Interfaces
{
    public interface IDishService
    {
        public Task<ApiResponse> CreateDishAsync(CreateDishV model);
        public Task<ApiResponse> UpdateDishAsync(CreateDishV model);
        public Task<ApiResponse> DeleteDishAsync(int Id);
        public Task<ApiResponse> AddPriceDishAsync(AddPriceDishDTO model);
        public Task<ApiResponse> DeletePriceDishAsync(int id);
        public Task<ApiResponse> GetAllDishAsync(int shopId, int pageIndex, int pageSize, string search);
        public Task<ApiResponse> GetAllDishByGroupMenu(string? search, int pageIndex, int pageSize, int? menuGroupId, int? shopId);
    }
}
