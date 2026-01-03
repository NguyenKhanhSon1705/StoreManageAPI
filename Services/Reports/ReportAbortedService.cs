using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Context;
using StoreManageAPI.DTO.Reports;
using StoreManageAPI.Helpers.Paging;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using StoreManageAPI.ViewModels.UserManager;

namespace StoreManageAPI.Services.Reports
{
    public class ReportAbortedService
        (
            ILogger<ReportAbortedService> logger,
            DataStore context
        ) : IReportAbortedService
    {
        private readonly ILogger<ReportAbortedService> logger = logger;
        private readonly DataStore context = context;

        public async Task<ApiResponse> GetAllReportAbortedAsync(ReportFilterDTO model)
        {
            try
            {
                var shop = await context.Shop.FindAsync(model.shop_id);
                if (shop == null)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy thông tin cửa hàng",
                    };
                }
                var listAborted = context.AbortedTables
                                    .Include(inc => inc.user)
                                    .Include(inc => inc.Table)
                                    .ThenInclude(tinc => tinc.Areas)
                                    .Where(w => w.shop_id == shop.Id)
                                    .AsQueryable();
                if (model.start_date.HasValue && model.end_date.HasValue)
                {
                    listAborted = listAborted.Where(a => a.aborted_date >= model.start_date.Value
                                                         && a.aborted_date <= model.end_date.Value);
                }

                if (!string.IsNullOrEmpty(model.employee_id)) // Lọc theo ID nhân viên
                {
                    listAborted = listAborted.Where(a => a.user.Id == model.employee_id);
                }

                listAborted = listAborted.OrderBy(o => o.aborted_date);

                var resultAbort = listAborted.Select(a => new ReportAbortedReturnDTO
                {
                    id = a.id,
                    user_name = a.user.FullName ?? a.user.Email,
                    table_name = a.Table.NameTable,
                    area_name = a.Table.Areas.AreaName,
                    reason_abort = a.reason_abort,
                    total_money = a.total_moneny,
                    total_quantity = a.total_quantity_dish,
                    created_table_at = a.create_table_date,
                    aborted_date = a.aborted_date
                });

                var result = await Paging<ReportAbortedReturnDTO, ReportAbortedReturnDTO>.CreateAsync(resultAbort, model.page_index, model.limit);
                var data = new Paging<ReportAbortedReturnDTO, ReportAbortedReturnDTO>(result.Items, result.TotalCount, result.PageIndex, result.PageSize);

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Lấy dữ liệu thành công",
                    Data = data,
                    IsSuccess = true
                }; 

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in ReportAbortedService/GetAllReportAbortedAsync: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in ReportAbortedService/GetAllReportAbortedAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> ShowAborteDetailAsync(int id)
        {
            try
            {
                var abortDetail = await context.AbortedTablesDish
                                    .Include(inc => inc.dish)
                                    .Include(inc => inc.dishPriceVersion)
                                    .Where(w => w.aborted_table_id == id)
                                    .Select(s => new ReportAbortedDetailReturnDTO
                                    {
                                       aborted_id = s.aborted_table_id,
                                        dish_id = s.dish_id,
                                        dish_name = s.dish.Dish_Name,
                                        image = s.dish.Image,
                                        price = s.dishPriceVersion.selling_price,
                                        quantity = s.quantity,
                                    }).ToListAsync();
                return new ApiResponse
                {
                    Message = "Thông tin chi tiết",
                    StatusCode = 200,
                    Data = abortDetail,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in ReportAbortedService/ShowAborteDetailAsync: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in ReportAbortedService/ShowAborteDetailAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
    }
}
