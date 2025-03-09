using StoreManageAPI.DTO.Dish;
using StoreManageAPI.ModelReturnData;

namespace StoreManageAPI.Services.Interfaces
{
    public interface IImportExcelService
    {
        public Task<ApiResponse> ImportExcelDishAsync(List<ImportDishDTO> model , int shop_id);
    }
}
