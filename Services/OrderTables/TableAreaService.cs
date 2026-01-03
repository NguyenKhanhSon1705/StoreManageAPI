using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreManageAPI.Context;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Ordertables;

namespace StoreManageAPI.Services.OrderTables
{
    public class TableAreaService
        (
        DataStore context,
        ILogger<TableAreaService> logger
        ) : ITableAreaService
    {
        private readonly DataStore context = context;
        private readonly ILogger<TableAreaService> logger = logger;
        public async Task<ApiResponse> GetTableByArea(int areaId)
        {
            try
            {
                var TableByArea = await context.Tables.Where(w => w.AreaId == areaId)
                    .Select(s => new TableByAreaV
                    {
                        NameTable = s.NameTable,
                        areaName = s.Areas.AreaName,
                        HasHourlyRate = s.HasHourlyRate,
                        Id = s.Id,
                        IsActive = s.IsActive,
                        IsBooking = s.IsBooking,
                        PriceOfMunite = s.PriceOfMunite,
                        TimeEnd = s.TimeEnd,
                        TimeStart = s.TimeStart,
                    })
                    .ToListAsync();

                if (TableByArea == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy khu vực/Bàn",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ApiResponse
                {
                    Data = TableByArea,
                    StatusCode = 200,
                    IsSuccess = true,
                    Message = "Lấy dữ liệu thành công"
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in TableAreaService/GetTableByArea: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in TableAreaService/GetTableByArea: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
    }
}
