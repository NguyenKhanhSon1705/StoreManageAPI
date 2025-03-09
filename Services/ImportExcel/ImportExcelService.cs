using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Context;
using StoreManageAPI.DTO.Dish;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Interfaces;

namespace StoreManageAPI.Services.ImportExcel
{
    public class ImportExcelService
        (
        DataStore context,
        ILogger<ImportExcelService> logger
        )
        : IImportExcelService
    {
        private readonly DataStore context = context;
        private readonly ILogger<ImportExcelService> logger = logger;

        public async Task<ApiResponse> ImportExcelDishAsync(List<ImportDishDTO> model, int shop_id)
        {
            try
            {

                var shop = await context.Shop.FindAsync(shop_id);
                if (shop == null)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Shop not found",
                    };
                }

                foreach (var item in model)
                {
                    string[] menu_groups = item.menu_group.Split(",");

                    

                    foreach (var menu_group in menu_groups)
                    {
                        var exitsMenu = await context.MenuGroups.Where(x => x.Name == menu_group.Trim()).FirstOrDefaultAsync();
                        if (exitsMenu == null)
                        {
                            var menu = new MenuGroup
                            {
                                Name = menu_group,
                                ShopId = shop_id,
                                Status = true,
                                Order = 0
                            };
                            context.MenuGroups.Add(menu);
                        }
                    }
                    string[] price_old = item.selling_price_old.Split(",");


                }

                await context.SaveChangesAsync();

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Import excel success",
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in ImportExcelService/ImportExcelDishAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in ImportExcelService/ImportExcelDishAsync: {ex.Message} at {DateTime.UtcNow}",
                };

            }
        }
    }
}
