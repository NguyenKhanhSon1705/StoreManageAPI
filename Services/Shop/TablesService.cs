using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Context;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Shopes;
using System.Security.Claims;

namespace StoreManageAPI.Services.Shop
{
    public class TablesService
        (
            DataStore context,
            ILogger<TablesService> logger,
            IMapper mapper
        ) : ITablesService
    {
        private readonly DataStore context = context;
        private readonly ILogger<TablesService> logger = logger;
        private readonly IMapper mapper = mapper;

        public Task<ApiResponse> ActiveTables(int idTable)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> CreateTableAsync(CreateTablesV model)
        {
            try
            {
                var area = await context.Areas.FindAsync(model.AreaId);
                if (area == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy khu vực",
                        StatusCode = 404
                    };
                }

                var exitsTableName = await context.Tables.AnyAsync(t => t.NameTable == model.NameTable && t.AreaId == model.AreaId);
                if(exitsTableName)
                {
                    return new ApiResponse
                    {
                        Message = "Tên bàn đã tồn tại",
                        StatusCode = 404
                    };
                }
                var newTable = new Tables
                {
                    NameTable = model.NameTable,
                    HasHourlyRate = model.HasHourlyRate,
                    AreaId = model.AreaId,
                    PriceOfMunite = model.PriceOfMunite,
                };


                context.Tables.Add(newTable);
                await context.SaveChangesAsync();

                var result = mapper.Map<TableInfoV>(newTable);

                result.AreaName = area.AreaName;

                return new ApiResponse
                {
                    Message = "Tạo bàn thành công",
                    IsSuccess = true,
                    StatusCode = 201,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in TablesService/CreateTableAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in TablesService/CreateTableAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> DeleteTableAsync(int idTable)
        {
            try
            {
                var table = await context.Tables.FindAsync(idTable);
                if( table == null )
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy bàn",
                        StatusCode = 404
                    };
                }
                context.Tables.Remove( table );
                await context.SaveChangesAsync();

                return new ApiResponse
                {
                    Message = "Xóa bàn thành công",
                    IsSuccess = true,
                    StatusCode = 200,
                    Data = table.Id
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in TablesService/DeleteTableAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in TablesService/DeleteTableAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> GetListTableAsync(int idShop)
        {
            try
            {
                var shop = await context.Shop.FindAsync(idShop);
                if( shop == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy cửa hàng",
                        StatusCode = 404
                    };
                }

                var result = await context.Tables
                    .Where(ar => ar.Areas.ShopId == shop.Id)
                    .OrderBy(o => o.Areas.AreaName)
                    .Select(x => new TableInfoV
                    {
                        Id = x.Id ,
                        AreaId = x.AreaId ,
                        NameTable = x.NameTable,
                        AreaName = x.Areas.AreaName,
                        HasHourlyRate = x.HasHourlyRate,
                        PriceOfMunite = x.PriceOfMunite,
                    })
                    .ToListAsync();

                return new ApiResponse
                {
                    Message = "Lấy dữ liệu thành công",
                    StatusCode = 200,
                    Data = result,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in TablesService/GetListTableAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in TablesService/GetListTableAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> GetTableByIdAsync(int idTable)
        {
            try
            {


                var result = await context.Tables.Include(t => t.Areas)
                    .Select(x => new TableInfoV
                    {
                        AreaName = x.Areas.AreaName , 
                        NameTable = x.NameTable , 
                        Id = x.Id , 
                        IsActive = x.IsActive ,
                        HasHourlyRate = x.HasHourlyRate , 
                        PriceOfMunite= x.PriceOfMunite ,
                    }).FirstOrDefaultAsync();
                    

                return new ApiResponse
                {
                    Message = "Lấy dữ liệu thành công",
                    StatusCode = 200,
                    Data = result,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in TablesService/GetTableById: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in TablesService/GetTableById: {ex.Message} at {DateTime.UtcNow}",
                };
            }

        }

        public async Task<ApiResponse> UpdateTableAsync(CreateTablesV model)
        {
            try
            {
                var area = await context.Areas.FindAsync(model.AreaId);
                if (area == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy khu vực",
                        StatusCode = 404
                    };
                }

                var table = await context.Tables.FindAsync(model.Id);
                if(table == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy bàn",
                        StatusCode = 404
                    };
                }

                table.AreaId = area.Id;
                table.PriceOfMunite = model.PriceOfMunite;
                table.NameTable = model.NameTable;
                table.HasHourlyRate = model.HasHourlyRate;

                context.Tables.Update(table);
                await context.SaveChangesAsync();
                
                var result = mapper.Map<TableInfoV>(table);
                result.AreaName = area.AreaName;

                return new ApiResponse
                {
                    Data = result,
                    IsSuccess = true,
                    Message = "Cập nhật bàn thành công",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in TablesService/UpdateTableAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in TablesService/UpdateTableAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
    }
}
