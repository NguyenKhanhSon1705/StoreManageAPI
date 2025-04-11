using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Context;
using StoreManageAPI.DTO.Order;
using StoreManageAPI.DTO.Payment;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Ordertables;
using StoreManageAPI.ViewModels.Shopes;
using StoreManageAPI.Websoket;
using System.Security.Claims;
namespace StoreManageAPI.Services.OrderTables
{
    public class TableDishService 
        (
        ILogger<TableDishService> logger,
        DataStore context,
        IHttpContextAccessor httpContext,
        UserManager<User> userManager,
        IMapper mapper,
        IHubContext<WsOrderTableArea> hubContext
        )
        : ITableDishService
    {
        private readonly ILogger<TableDishService> logger = logger;
        private readonly DataStore context = context;
        private readonly IHttpContextAccessor httpContext = httpContext;
        private readonly UserManager<User> userManager = userManager;
        private readonly IMapper mapper = mapper;
        private readonly IHubContext<WsOrderTableArea> hubContext = hubContext;

        private async Task NotifyWebsoket(int areaId)
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
            await hubContext.Clients.Group($"Area-{areaId}").SendAsync("TableUpdated", TableByArea);
        }

        public async Task<ApiResponse> GetInfoCheckoutAsync(int table_id, int shop_id)
        {
            try
            {
                var userId = httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var currentUser = await userManager.FindByIdAsync(userId ?? "");

                if (currentUser == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy người dùng hiện tại",
                        StatusCode = 404
                    };
                }

                var curShop = await context.Shop.FindAsync(shop_id);
                if (curShop == null)
                {
                    return new ApiResponse
                    {
                        Message = "Thông tin cửa hàng không hợp lệ",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                var currTable = await context.Tables.FindAsync(table_id);
                if (currTable == null)
                {
                    return new ApiResponse
                    {
                        Message = "Bàn không tồn tại",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                if (!currTable.IsActive)
                {
                    return new ApiResponse
                    {
                        Message = "Bàn chưa được tạo",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                var tableDishes = await context.TableDishs
                    .Include(inc => inc.dish)
                    .Where(w => w.tableId == table_id).ToListAsync();
                int? total_quantity = 0;
                decimal? total_money = 0;

                List<DishInfoV> listdish = new List<DishInfoV>();
                foreach (var item in tableDishes)
                {
                    var price = await context.DishPriceVersions.Where(w => w.dish_id == item.dishId && w.status == true).FirstOrDefaultAsync();
                    total_quantity += item.quantity;

                    total_money += item.quantity * price.selling_price;
                    var newDish = new DishInfoV
                    {
                        notes = item.notes,
                        dish_Name = item.dish.Dish_Name,
                        quantity = item.quantity,
                        selling_Price = price.selling_price,
                    };
                    listdish.Add(newDish);
                }

                await context.SaveChangesAsync();
                var areaName = await context.Tables
                    .Where(w => w.Id == currTable.Id)
                    .Include(inc => inc.Areas)
                    .Select(s => s.Areas.AreaName)
                    .FirstOrDefaultAsync();

                var result = new PaymantInfoDTO
                {
                    total_money = total_money,
                    total_quantity = total_quantity,
                    table_name = currTable.NameTable,
                    staff_name = currentUser.FullName ?? currentUser.Email,
                    area_name = areaName,
                    shop_name = curShop.ShopName,
                    hotline = curShop.ShopPhone,
                    address_shop = curShop.ShopAddress,
                    time_start = currTable.TimeStart,
                    time_end = DateTime.UtcNow,
                    listDish = listdish

                };
                return new ApiResponse { 
                Message = "Thông tin thanh toán",
                IsSuccess = true,
                StatusCode = 200,
                Data = result
                };
                
            }
            catch ( Exception ex )
            {
                logger.LogError($"There are something error in TableDishService/PaymentAsync: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in TableDishService/PaymentAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
        public async Task<ApiResponse> PaymentAsync(PaymentDTO model , TransactionsDTO? tst = null)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var userId = httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var currentUser = await userManager.FindByIdAsync(userId ?? "");

                if (currentUser == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy người dùng hiện tại",
                        StatusCode = 404
                    };
                }

                var curShop = await context.Shop.FindAsync(model.shop_id);
                if(curShop == null)
                {
                    return new ApiResponse
                    {
                        Message = "Thông tin cửa hàng không hợp lệ",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                var currTable = await context.Tables.FindAsync(model.table_Id);
                if (currTable == null)
                {
                    return new ApiResponse
                    {
                        Message = "Bàn không tồn tại",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                if (!currTable.IsActive)
                {
                    return new ApiResponse
                    {
                        Message = "Bàn chưa được tạo",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }


                var bill = new Bill
                {
                    bill_code = DateTime.UtcNow.Ticks.ToString(),
                    user_id  = userId,
                    table_id = currTable.Id,
                    shop_id = model.shop_id,
                    time_end = DateTime.UtcNow,
                    time_start = currTable.TimeStart,
                    payment_method = model.payment_method,

                };

                var tableDishes = await context.TableDishs
                    .Include(inc => inc.dish)
                    .Where(w => w.tableId == model.table_Id).ToListAsync();
                int? total_quantity = 0;
                decimal? total_money = 0;

                List<DishInfoV> listdish = new List<DishInfoV>();
                foreach (var item in tableDishes)
                {
                    var price = await context.DishPriceVersions.Where(w => w.dish_id == item.dishId && w.status == true).FirstOrDefaultAsync();
                    total_quantity += item.quantity;

                    total_money += item.quantity * price.selling_price;
                    var newDish = new DishInfoV
                    {
                        notes = item.notes,
                        dish_Name = item.dish.Dish_Name,
                        quantity = item.quantity,
                        selling_Price = price.selling_price,
                    };
                    listdish.Add(newDish);
                    var billDetails = new BillDetails
                    {
                        Bill = bill,
                        notes = item.notes,
                        quantity = item.quantity,
                        selling_price_id = price.id,
                        dish_id = item.dishId
                    };
                    context.BillDetails.Add(billDetails);
                }
                bill.total_quantity = total_quantity;
                bill.total_money = total_money;


                currTable.TimeStart = null;
                currTable.TimeEnd = null;
                currTable.IsActive = false;

                context.Bills.Add(bill);
                context.Tables.Update(currTable);
                context.TableDishs.RemoveRange(tableDishes);

                if(tst != null)
                {
                    var trans = mapper.Map<Transactions>(tst);
                    trans.Bill = bill;
                    context.Transactions.Add(trans);
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                //var areaName = await context.Tables
                //    .Where(w => w.Id == currTable.Id)
                //    .Include(inc => inc.Areas)
                //    .Select(s => s.Areas.AreaName)
                //    .FirstOrDefaultAsync();

                 var area = await context.Tables
                     .Where(w => w.Id == currTable.Id)
                     .Include(inc => inc.Areas)
                     .Select(s => new
                     {
                         name = s.Areas.AreaName , 
                         id = s.Areas.Id
                     })
                     .FirstOrDefaultAsync();

                // websocket
                await NotifyWebsoket(area.id);

                var result = new PaymantInfoDTO
                {
                    bill_number = bill.id,
                    total_money = total_money,
                    total_quantity = total_quantity,
                    table_name = currTable.NameTable,
                    staff_name = currentUser.FullName ?? currentUser.Email,
                    area_name = area.name,
                    shop_name = curShop.ShopName,
                    hotline = curShop.ShopPhone,
                    address_shop = curShop.ShopAddress,
                    time_start = currTable.TimeStart,
                    time_end = DateTime.UtcNow,
                    listDish = listdish
                };

                return new ApiResponse
                {
                    Message = "Thanh toán thành công",
                    IsSuccess = true,
                    StatusCode = 200,
                    Data = result

                };
            }catch ( Exception ex )
            {
                await transaction.RollbackAsync();
                logger.LogError($"There are something error in TableDishService/PaymentAsync: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in TableDishService/PaymentAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
        public async Task<ApiResponse> AbortedDishOnTableAsync(AbortedTableDTO model)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var userId = httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currentUser = await userManager.FindByIdAsync(userId ?? "");
                if (currentUser == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy người dùng hiện tại",
                        StatusCode = 404
                    };
                }
                var checkShop = await context.Shop.FindAsync(model.shop_id);
                if(checkShop == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy thông tin cửa hàng",
                        StatusCode = 404
                    };
                }
                var currTable = await context.Tables.FindAsync(model.table_Id);
                if (currTable == null)
                {
                    return new ApiResponse
                    {
                        Message = "Bàn không tồn tại",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                if(!currTable.IsActive)
                {
                    return new ApiResponse
                    {
                        Message = "Bàn chưa được tạo",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                var abortedTable = new AbortedTable
                {
                    shop_id = checkShop.Id,
                    aborted_date = DateTime.UtcNow,
                    reason_abort = model.reason_abort,
                    table_id = currTable.Id,
                    create_table_date = currTable.TimeStart,
                    user_id = userId,
                    total_quantity_dish = model.total_quantity,
                    total_moneny = model.total_money
                };
                context.AbortedTables.Add(abortedTable);

                var tableDishes = await context.TableDishs.Where(w => w.tableId ==  model.table_Id).ToListAsync();
                
                foreach (var item in tableDishes)
                {
                    var price = await context.DishPriceVersions.Where(w => w.dish_id == item.dishId && w.status == true).FirstOrDefaultAsync();
                    var abortedTableDish = new AbortedTableDish
                    {
                        abortedTable = abortedTable,
                        quantity = item.quantity,
                        selling_price_id = price.id,
                        dish_id = item.dishId
                    };
                    context.AbortedTablesDish.Add(abortedTableDish);
                }
                currTable.TimeStart = null;
                currTable.IsActive = false;
                context.Tables.Update(currTable);
                context.TableDishs.RemoveRange(tableDishes);

                await context.SaveChangesAsync();

                var area = await context.Tables
                   .Where(w => w.Id == currTable.Id)
                   .Include(inc => inc.Areas)
                   .Select(s => new
                   {
                       name = s.Areas.AreaName,
                       id = s.Areas.Id
                   })
                   .FirstOrDefaultAsync();
                // websocket
                await NotifyWebsoket(area.id);
                await transaction.CommitAsync();

                return new ApiResponse
                {
                    Message = "Hủy thành công",
                    IsSuccess = true,
                    StatusCode = 200,
                };
            }catch ( Exception ex )
            {
                await transaction.RollbackAsync();
                logger.LogError($"There are something error in TableDishService/AbortedDishOnTableAsync: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in TableDishService/AbortedDishOnTableAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> ChangeTableDish(int tableIdOld , int tableIdNew)
        {
          
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var checkTableNew = await context.Tables.FindAsync(tableIdNew);
                if (checkTableNew == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy bàn mới.",
                        StatusCode = 404
                    };
                }

                if (checkTableNew.IsActive)
                {
                    return new ApiResponse
                    {
                        Message = "Bàn này đã tồn tại, vui lòng chọn bàn trống.",
                        StatusCode = 400
                    };
                }

                var tableOld = await context.Tables.FindAsync(tableIdOld);
                if (tableOld == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy bàn cũ.",
                        StatusCode = 404
                    };
                }

                // Cập nhật thông tin bàn mới và bàn cũ
                checkTableNew.TimeStart = tableOld.TimeStart;
                checkTableNew.IsActive = true;
                context.Tables.Update(checkTableNew);

                tableOld.IsActive = false;
                tableOld.TimeStart = null;
                context.Tables.Update(tableOld);
                var tableDishOld = await context.TableDishs
                       .Where(td => td.tableId == tableIdOld)
                       .ToListAsync();

                context.TableDishs.RemoveRange(tableDishOld);

                foreach (var item in tableDishOld)
                {
                    var newItem = new TableDishs
                    {
                        tableId = tableIdNew,
                        dishId = item.dishId,
                        quantity = item.quantity,
                        selling_Price = item.selling_Price,
                        notes = item.notes
                    };

                    context.TableDishs.Add(newItem); // Thêm món mới với tableIdNew
                }
                // Lưu thay đổi
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                    // Lấy thông tin món ăn và khu vực của bàn mới
                var groupedResult = await context.TableDishs
                .Where(w => w.tableId == checkTableNew.Id)
                .Include(inc => inc.dish)
                .Select(s => new DishInfoV
                {
                    Id = s.dish.Id,
                    dish_Name = s.dish.Dish_Name,
                    image = s.dish.Image,
                    selling_Price = s.selling_Price,
                    quantity = s.quantity,
                    notes = s.notes,
                })
                .ToListAsync();

                var areaName = await context.Tables
                    .Where(w => w.Id == checkTableNew.Id)
                    .Include(inc => inc.Areas)
                    .Select(s => s.Areas.AreaName)
                    .FirstOrDefaultAsync();

                var result = new TableDishInfoV
                {
                    dish = groupedResult,
                    TimeStart = checkTableNew.TimeStart,
                    TimeEnd = checkTableNew.TimeEnd,
                    NameTable = checkTableNew.NameTable,
                    IsActive = checkTableNew.IsActive,
                    HasHourlyRate = checkTableNew.HasHourlyRate,
                    Id = checkTableNew.Id,
                    PriceOfMunite = checkTableNew.PriceOfMunite,
                    areaName = areaName
                };
                return new ApiResponse
                {
                    Message = $"Chuyển sang bàn {checkTableNew.NameTable} thành công",
                    IsSuccess = true,
                    StatusCode = 200,
                    Data = result
                };
            }
            catch ( Exception ex )
            {
                await transaction.RollbackAsync();
                logger.LogError($"There are something error in TableDishService/ChangeTableDish: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in TableDishService/ChangeTableDish: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
        public async Task<ApiResponse> GetInfoDishCurrentTable(int tableId)
        {
            try
            {
                var table = await context.Tables.FindAsync(tableId);
                if( table == null )
                {
                    return new ApiResponse
                    {
                        Message = "Bàn không tồn tại",
                        StatusCode = 404
                    };
                }

                var groupedResult = await context.TableDishs
                       .Where(w => w.tableId == tableId)
                       .Include(inc => inc.dish)
                       .Select(s => new DishInfoV
                       {
                           Id = s.dish.Id,
                           dish_Name = s.dish.Dish_Name,
                           image = s.dish.Image,
                           selling_Price = s.selling_Price ,
                           quantity = s.quantity,
                           notes = s.notes,
                       })
                       .ToListAsync();
                var areaName = await context.Tables
                    .Where(w => w.Id == tableId)
                    .Include(inc => inc.Areas)
                    .Select(s => s.Areas.AreaName)
                    .FirstOrDefaultAsync();
                var result = new TableDishInfoV
                {
                    dish = groupedResult,
                    TimeStart = table.TimeStart,
                    TimeEnd = table.TimeEnd,
                    NameTable = table.NameTable,
                    IsActive = table.IsActive,
                    HasHourlyRate = table.HasHourlyRate,
                    Id = table.Id,
                    PriceOfMunite = table.PriceOfMunite,
                    areaName = areaName
                };

                return new ApiResponse
                {
                    Message = "Thông tin bàn",
                    IsSuccess = true,
                    StatusCode = 200,
                    Data = result
                };
            }catch (Exception ex)
            {
                logger.LogError($"There are something error in TableDishService/GetInfoCurrentTable: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in TableDishService/GetInfoCurrentTable: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
        public async Task<ApiResponse> OpenTableAsync(CreateTableDishV model)
        {
            try
            {
                var curTable = await context.Tables.FindAsync(model.tableId);
                if (curTable.IsActive)
                {
                    return new ApiResponse
                    {
                        Message = "Bàn này đã tồn tại món ăn, Vui lòng thanh toán",
                        StatusCode = 409,
                    };
                }
                if(model.listDishId == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không thể tạo bàn trống",
                        StatusCode = 409,
                    };
                }

                foreach (var item in model.listDishId)
                {
                    var newTablDish = new TableDishs
                    {
                        dishId = item.key,
                        tableId = curTable.Id,
                        notes = item.notes,
                        quantity = item.quantity,
                        selling_Price = item.selling_Price,
                    };
                    context.TableDishs.Add(newTablDish);

                }
                curTable.IsActive = true;
                curTable.TimeStart = DateTime.UtcNow;
                context.Tables.Update(curTable);
                await context.SaveChangesAsync();

                //var groupedResult = context.TableDishs
                //      .Where(w => w.tableId == model.tableId)
                //      .Include(inc => inc.dish)
                //      .Select(s => new DishInfoV
                //      {
                //          Id = s.dish.Id,
                //          dish_Name = s.dish.Dish_Name,
                //          image = s.dish.Image,
                //          selling_Price = s.selling_Price,
                //          quantity = s.quantity,
                //          notes = s.notes,
                //      })
                //      .ToList();

                var area = await context.Tables
                    .Where(w => w.Id == model.tableId)
                    .Include(inc => inc.Areas)
                    .Select(s => new
                    {
                        name = s.Areas.AreaName , 
                        id = s.Areas.Id
                    })
                    .FirstOrDefaultAsync();

                // websocket
                await NotifyWebsoket(area.id);

                var result = new TableDishInfoV
                {
                    TimeStart = curTable.TimeStart,
                    TimeEnd = curTable.TimeEnd,
                    NameTable = curTable.NameTable,
                    IsActive = curTable.IsActive,
                    HasHourlyRate = curTable.HasHourlyRate,
                    Id = curTable.Id,
                    PriceOfMunite = curTable.PriceOfMunite,
                    areaName = area.name
                };

                
                return new ApiResponse
                {
                    Message = "Tạo bàn món ăn thành công",
                    IsSuccess = true,
                    StatusCode = 201,
                    Data = result
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in TableDishService/OpenTableAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in TableDishService/OpenTableAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
        public async Task<ApiResponse> UpdateTableDishAsync(CreateTableDishV model)
        {
            try
            {
                var curTable = await context.Tables.FindAsync(model.tableId);
                var checkActiveTable = curTable.IsActive == false;
                if (checkActiveTable)
                {
                    return new ApiResponse
                    {
                        Message = "Bàn này chưa được mở vui lòng mở bàn",
                        StatusCode = 404,
                    };
                }
                if (model.listDishId == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không thể tạo bàn trống",
                        StatusCode = 404,
                    };
                }
                var tableDish  = await context.TableDishs.Where(w => w.tableId == model.tableId).ToListAsync();
                context.TableDishs.RemoveRange(tableDish);

                foreach (var item in model.listDishId)
                {
                    var newTablDish = new TableDishs
                    {
                        dishId = item.key,
                        tableId = curTable.Id,
                        notes = item.notes,
                        quantity = item.quantity,
                        selling_Price = item.selling_Price,
                    };
                    context.TableDishs.Add(newTablDish);
                }
                await context.SaveChangesAsync();


                var areaName = await context.Tables
                    .Where(w => w.Id == model.tableId)
                    .Include(inc => inc.Areas)
                    .Select(s => s.Areas.AreaName)
                    .FirstOrDefaultAsync();
                var result = new TableDishInfoV
                {
                    TimeStart = curTable.TimeStart,
                    TimeEnd = curTable.TimeEnd,
                    NameTable = curTable.NameTable,
                    IsActive = curTable.IsActive,
                    HasHourlyRate = curTable.HasHourlyRate,
                    Id = curTable.Id,
                    PriceOfMunite = curTable.PriceOfMunite,
                    areaName = areaName
                };

                return new ApiResponse
                {
                    Message = "Cập nhận bàn thành công",
                    IsSuccess = true,
                    StatusCode = 201,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in UpdateTableDishAsync/OpenTableAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in UpdateTableDishAsync/OpenTableAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

       
    }
}
