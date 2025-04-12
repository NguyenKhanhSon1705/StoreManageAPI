using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Context;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Shopes;

namespace StoreManageAPI.Services.Shop
{
    public class AreasService 
        (
        DataStore context,
        ILogger<AreasService> logger
        )
        : IAreasService
    {
        private readonly DataStore context = context;
        private readonly ILogger<AreasService> logger = logger;
        public async Task<ApiResponse> CreateAreaAsync(AreasV model)
        {
            try
            {
                var result = await context.Areas
                    .Where(ar => ar.ShopId == model.IdShop)
                    .AnyAsync(ar => ar.AreaName == model.AreaName);
                    
                if (result)
                {
                    return new ApiResponse
                    {
                        Message = "Tên khu vực đã tồn tại",
                        StatusCode = 409
                    };
                }

                var newArea = new Areas
                {
                    AreaName = model.AreaName.Trim(),
                    ShopId = model.IdShop
                };
                context.Areas.Add(newArea);
                await context.SaveChangesAsync();

                return new ApiResponse
                {
                    Message = "Tạo khu vực thành công",
                    StatusCode = 201,
                    IsSuccess = true,
                    Data = newArea
                };

            }catch ( Exception ex )
            {
                logger.LogError($"There are something error in AreasService/CreateAreaAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in AreasService/CreateAreaAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> DeleteAreaAsync(int id)
        {
            try
            {
                var curArea = await context.Areas.FindAsync(id);
                if( curArea == null)
                {
                    return new ApiResponse
                    {
                        Message = "Tạo khu vực thành công",
                        StatusCode = 404
                    };
                }

                context.Areas.Remove(curArea);
                await context.SaveChangesAsync();
                return new ApiResponse
                {
                    Data = curArea,
                    Message = "Xóa khu vực thành công",
                    IsSuccess =true,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in AreasService/DeleteAreaAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in AreasService/DeleteAreaAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> GetAreaByIdAsync(int id)
        {
            try
            {
                var curArea = await context.Areas.FindAsync(id);
                if(curArea == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy khu vực",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ApiResponse
                {
                    Data = curArea,
                    StatusCode = 200,
                    IsSuccess = true,
                    Message = "Lấy dữ liệu thành công"
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in AreasService/GetAreaByIdAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in AreasService/GetAreaByIdAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> GetListAreaAsync(int idShop)
        {
            try
            {
                var listArea = await context.Areas.Where(ar => ar.ShopId == idShop).ToListAsync();
                if(listArea == null)
                {
                    return new ApiResponse
                    {
                        StatusCode = 404,
                        Message = "Không có dữ liệu",

                    };
                }

                return new ApiResponse
                {
                    Message = "Lấy dữ liệu thành công",
                    IsSuccess = true,
                    StatusCode = 200,
                    Data = listArea
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in AreasService/GetListAreaAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in AreasService/GetListAreaAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> UpdateAreaAsync(AreasV model)
        {
            try
            {

                var curArea = await context.Areas.FindAsync(model.Id);
        
                if(curArea == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy khu vực",
                        StatusCode = 404
                    };
                }

                var checkNameArea = await context.Areas.Where(ar => ar.ShopId == curArea.ShopId).AnyAsync(ar => ar.AreaName == model.AreaName);

                if (checkNameArea)
                {
                    return new ApiResponse
                    {
                        Message = "Tên khu vực đã tồn tại",
                        StatusCode = 409
                    };
                }
                curArea.AreaName = model.AreaName;
                context.SaveChanges();
                return new ApiResponse
                {
                    Message = "Cập nhật tên khu vực thành công",
                    StatusCode = 200,
                    Data = curArea,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in AreasService/UpdateAreaAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in AreasService/UpdateAreaAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
    }
}
