using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Context;
using StoreManageAPI.DTO.Reports;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Dish;
using StoreManageAPI.ViewModels.Ordertables;

namespace StoreManageAPI.Services.Reports
{
    public class StatisticalService
        (
        ILogger<StatisticalService> logger,
        DataStore context
        ) : IStatisticalService
    {
        private readonly ILogger<StatisticalService> logger = logger;
        private readonly DataStore context = context;

        public async Task<ApiResponse> RevenueReportAsync(FilterRevenueReportDTO model)
        {
            try
            {
                var shop = await context.Shop.FindAsync(model.shop_id);
                if (shop == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy thông tin cửa hàng",
                        StatusCode = 404
                    };
                }

                // Xác định khoảng thời gian
                var time_start = model.start_date ?? DateTime.MinValue;
                var time_end = model.end_date ?? DateTime.MaxValue;

                var billData = await context.Bills
                                    .Where(w => w.time_end >= time_start && w.time_end <= time_end && w.shop_id == model.shop_id)
                                    .GroupBy(_ => true) // Gom nhóm toàn bộ dữ liệu
                                    .Select(group => new
                                    {
                                        TotalRevenue = group.Sum(b => b.total_money),
                                        TotalBills = group.Count()
                                    })
                                    .FirstOrDefaultAsync();

                var total_revenue = billData?.TotalRevenue ?? 0;
                var total_bill = billData?.TotalBills ?? 0;

                // Doanh thu và số bàn đang xử lý
                var tabldish = await context.TableDishs
                                        .AsNoTracking()
                                        .AsSplitQuery()
                                        .Include(inc => inc.table)
                                        .ThenInclude(then => then.Areas)
                                        .Where(w => w.table.Areas.ShopId == model.shop_id)
                                        .ToListAsync();

                var pedding_revenue = tabldish.Sum(sum => sum.selling_Price);

                var pedding_bill = await context.Tables
                                .Include(inc => inc.Areas)
                                .Where(w => w.Areas.ShopId == model.shop_id && w.IsActive == true)
                                        .CountAsync();  

                // Thống kê bàn bị hủy
                var aborted_revenue = await context.AbortedTables
                    .Where(w => w.aborted_date >= time_start && w.aborted_date <= time_end && w.shop_id == model.shop_id)
                    .SumAsync(sum => sum.total_moneny);

                var aborted_bill = await context.AbortedTables
                    .Where(w => w.aborted_date >= time_start && w.aborted_date <= time_end && w.shop_id == model.shop_id)
                    .CountAsync();

                var total_aborted_dish = await context.AbortedTables
                    .Where(w => w.aborted_date >= time_start && w.aborted_date <= time_end && w.shop_id == model.shop_id)
                    .SumAsync(sum => sum.total_quantity_dish);

                // Doanh thu từ giao dịch online
                var transaction_online = await context.Transactions
                    .Include(inc => inc.Bill)
                    .Where(w => w.Bill.time_end >= time_start && w.Bill.time_end <= time_end && w.Bill.shop_id == model.shop_id)
                    .SumAsync(sum => sum.Bill.total_money);


                var dishHost = await context.BillDetails
                    .Include(inc => inc.Bill)
                    .Where(w => w.Bill.time_end >= time_start && w.Bill.time_end <= time_end && w.Bill.shop_id == model.shop_id)
                    .GroupBy(gr => gr.dish_id)
                    .Select(group => new
                    {
                        dish_id = group.Key,
                        total_quantity = group.Sum(b => b.quantity)
                    })
                    .OrderByDescending(p => p.total_quantity)
                    .Take(3)
                    .Join(
                        context.Dish
                        .Where(product => product.Price.status == true), // Lọc trước các sản phẩm thỏa mãn
                        billDetail => billDetail.dish_id,
                        product => product.Id,
                        (billDetail, product) => new DishInfoV
                        {
                            Id = product.Id,
                            dish_Name = product.Dish_Name,
                            quantity = billDetail.total_quantity,
                            image = product.Image,
                            selling_Price = product.Price.selling_price,
                        }
                    )
                    .ToListAsync();

                var dishIsNotHot = await context.BillDetails
                    .Include(inc => inc.Bill)
                    .Where(w => w.Bill.time_end >= time_start && w.Bill.time_end <= time_end && w.Bill.shop_id == model.shop_id)
                    .GroupBy(gr => gr.dish_id)
                    .Select(group => new
                    {
                        dish_id = group.Key,
                        total_quantity = group.Sum(b => b.quantity)
                    })
                    .OrderBy(p => p.total_quantity)
                    .Take(3)
                    .Join(
                        context.Dish.Where(product => product.Price.status == true), // Lọc trước các sản phẩm thỏa mãn
                        billDetail => billDetail.dish_id,
                        product => product.Id,
                        (billDetail, product) => new DishInfoV
                        {
                            Id = product.Id,
                            dish_Name = product.Dish_Name,
                            quantity = billDetail.total_quantity,
                            image = product.Image,
                            selling_Price = product.Price.selling_price,
                        }
                    )
                    .ToListAsync();

                var result = new RevenueReportReturnDTO
                {
                    total_nuvenue = total_revenue,
                    total_transaction_online = transaction_online,
                    pendding_nuvenue = pedding_revenue,

                    total_billed = total_bill,
                    pendding_bill = pedding_bill,

                    total_aborted_money = aborted_revenue,
                    total_aborted = aborted_bill,
                    total_aborted_dish = total_aborted_dish,

                    list_dish_hot = dishHost,
                    list_dish_not_hot = dishIsNotHot
                };


                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    IsSuccess = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in StatisticalService/RevenueReportAsync: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in StatisticalService/RevenueReportAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
    }
}
