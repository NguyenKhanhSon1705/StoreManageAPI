using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Context;
using StoreManageAPI.DTO.Reports;
using StoreManageAPI.Helpers.Paging;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Services.Interfaces;

namespace StoreManageAPI.Services.Reports
{
    public class ReportBillService
        (
        ILogger<ReportBillService> logger,
        DataStore context

        ) : IReportBillService
    {
        private readonly ILogger<ReportBillService> logger = logger;
        private readonly DataStore context = context;
        public async Task<ApiResponse> GetAllReportBillAsync(ReportFilterDTO model)
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
                var listBill = context.Bills
                                    .AsNoTracking()
                                    .Include(inc => inc.User)
                                    .Include(inc => inc.Tables)
                                    .ThenInclude(tinc => tinc.Areas)
                                    .Where(w => w.shop_id == shop.Id);

                if (model.start_date.HasValue || model.end_date.HasValue)
                {

                    DateTime startDate = model.start_date ?? DateTime.MinValue;
                    var endDate = model.end_date ?? DateTime.MaxValue;

                    listBill = listBill.Where(b => b.time_end >= startDate && b.time_end <= endDate);
                }

                if (!string.IsNullOrEmpty(model.employee_id)) // Lọc theo ID nhân viên
                {
                    listBill = listBill.Where(a => a.User.Id == model.employee_id);
                }

                if (!string.IsNullOrEmpty(model.search_bill_code))
                {
                    listBill = listBill.Where(a => a.bill_code == model.search_bill_code);
                }

                listBill = listBill.OrderByDescending(o => o.time_end);

                var resultBill = listBill.Select(a => new ReportBillReturnDTO
                {
                    id = a.id,
                    bill_code = a.bill_code,
                    user_name = a.User.FullName ?? a.User.Email,
                    table_name = a.Tables.NameTable,
                    area_name = a.Tables.Areas.AreaName,
                    total_money = a.total_money,
                    total_quantity = a.total_quantity,
                    time_start = a.time_start,
                    time_end = a.time_end,
                    VAT = a.VAT,
                    discount = a.discount,
                    paymentMethod = a.payment_method == 0 ? "Tiền mặt" : "Thẻ",
                });
                var result = await Paging<ReportBillReturnDTO, ReportBillReturnDTO>.CreateAsync(resultBill, model.page_index, model.limit);
                var data = new Paging<ReportBillReturnDTO, ReportBillReturnDTO>(result.Items, result.TotalCount, result.PageIndex, result.PageSize);


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
                logger.LogError($"There are something error in ReportBillService/GetAllReportBillAsync: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in ReportBillService/GetAllReportBillAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> ShowBillDetailAsync(int id)
        {
            try
            {
                var abortDetail = await context.BillDetails
                                    .Include(inc => inc.Dish)
                                    .Include(inc => inc.DishPriceVersion)
                                    .Where(w => w.bill_id == id)
                                    .Select(s => new DishBillInfo
                                    {
                                        dish_id = s.dish_id,
                                        dish_name = s.Dish.Dish_Name,
                                        image = s.Dish.Image,
                                        price = s.DishPriceVersion.selling_price,
                                        quantity = s.quantity,
                                        notes = s.notes
                                   }).ToListAsync();

                var transection = await context.Transactions
                                    .Where(w => w.bill_id == id)
                                    .FirstOrDefaultAsync();

                var result = new ReportBillDetailReturnDTO
                {
                    list_item = abortDetail,
                    transaction_status = transection?.status_name,
                    description = transection?.decription,
                    bank_code = transection?.bank_code,
                    payment_date = transection?.payment_date,
                    payment_method = transection == null ? "Tiền mặt" : "Thẻ",
                };
                return new ApiResponse
                {
                    Message = "Thông tin chi tiết",
                    StatusCode = 200,
                    Data = result,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in ReportBillService/ShowBillDetailAsync: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in ReportBillService/ShowBillDetailAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
    }
}
