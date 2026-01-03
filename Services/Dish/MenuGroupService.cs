using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mysqlx.Crud;
using StoreManageAPI.Context;
using StoreManageAPI.Helpers.Paging;
using StoreManageAPI.Mddleware;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Dish;
namespace StoreManageAPI.Services.Dish
{
    public class MenuGroupService
        (
            ILogger<MenuGroupService> logger,
            DataStore context,
            IMapper mapper,
            CloudinaryMiddle cloud
        )
        : IMenuGroupService
    {
        private readonly ILogger<MenuGroupService> logger = logger;
        private readonly IMapper mapper = mapper;
        private readonly DataStore context = context;
        private readonly CloudinaryMiddle cloud = cloud;
        public async Task<ApiResponse> CreateGroupMenuAsync(CreateMenuGroupV model)
        {
            try
            {
                var exitsGroupMenu = await context.MenuGroups.AnyAsync(t => t.Name == model.Name && t.ShopId == model.ShopId);
                if (exitsGroupMenu)
                {
                    return new ApiResponse
                    {
                        Message = "Tên nhóm món ăn này đã tồn tại",
                        StatusCode = 404
                    };
                }

                var shop = await context.Shop.FindAsync(model.ShopId);
                if (shop == null)
                {
                    return new ApiResponse
                    {
                        Message = "Thông tin cửa hàng không hợp lệ",
                        StatusCode = 404
                    };
                }
                var menuGroup = new MenuGroup
                {
                    Name = model.Name,
                    Description = model.Description,
                    ShopId = shop.Id,
                    Status = model.Status,
                    Order = model.Order,
                };

                if(model.Image != null)
                {
                    menuGroup.Image = await cloud.CloudinaryUploadImage(model.Image);
                }

                context.MenuGroups.Add(menuGroup);
                await context.SaveChangesAsync();

                var data = mapper.Map<MenuGroupV>(menuGroup);
                return new ApiResponse
                {
                    Message = "Tạo nhóm món ăn thành công",
                    StatusCode = 201,
                    Data = data,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in MenuGroupService/CreateGroupMenuAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in MenuGroupService/CreateGroupMenuAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
        public async Task<ApiResponse> DeleteGroupMenuAsync(int Id)
        {
            try
            {
                var menuGroup = await context.MenuGroups.FindAsync(Id);
                if(menuGroup == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy nhóm món ăn",
                        StatusCode = 404
                    };
                }
                context.MenuGroups.Remove(menuGroup ?? new MenuGroup());
                await context.SaveChangesAsync();
                return new ApiResponse
                {
                    StatusCode = 200,
                    Message = $"Xóa nhóm món {menuGroup.Name} thành công",
                    Data = menuGroup,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in MenuGroupService/DeleteGroupMenuAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in MenuGroupService/DeleteGroupMenuAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> GetAllGroupMenusAsync(int shopId, int _pageIndex = 1, int limit = 10, string search = "")
        {
            try
            {
                var menuGroups =  context.MenuGroups.Where(m => m.ShopId == shopId).AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    menuGroups = menuGroups.Where(m => m.Name.Contains(search));
                }
                menuGroups = menuGroups.OrderBy(o => o.Order);

                var pageIndex = _pageIndex;
                var pageSize = limit;

                var result = await Paging<MenuGroup, MenuGroup>.CreateAsync(menuGroups, pageIndex, pageSize);
                var list = new Paging<MenuGroupV, MenuGroupV>(mapper.Map<List<MenuGroupV>>(result.Items), result.TotalCount, result.PageIndex, result.PageSize);

                return new ApiResponse
                {
                    Message = "Danh sách",
                    StatusCode = 200,
                    Data = list,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in MenuGroupService/GetAllGroupMenusAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in MenuGroupService/GetAllGroupMenusAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }

        }

        public async Task<ApiResponse> GetAllNameMenuGroup(int shopId)
        {
            try
            {
                var menuGroup = await context.MenuGroups
                    .Where(w => w.ShopId == shopId)
                    .Where(w => w.Status == true)
                    .Select(x => new ObMenuGroupV
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Image = x.Image
                    })
                    .ToListAsync();
                //var data = mapper.Map<MenuGroupV>(menuGroup);
                return new ApiResponse
                {
                    Message = "Lấy dữ liệu thành công",
                    StatusCode = 200,
                    Data = menuGroup,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in MenuGroupService/GetAllNameMenuGroup: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in MenuGroupService/GetAllNameMenuGroup: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> UpdateGroupMenuAsync(CreateMenuGroupV model)
        {
            try
            {
                var menuGroup = await context.MenuGroups.FindAsync(model.Id);
                if (menuGroup.ShopId == model.ShopId && menuGroup.Name == model.Name)
                {
                    return new ApiResponse
                    {
                        Message = "Tên nhóm món ăn này đã tồn tại",
                        StatusCode = 404
                    };
                }

                menuGroup.Name = model.Name;
                menuGroup.Order = model.Order;
                menuGroup.Status = model.Status;
                menuGroup.Description = model.Description;

                if (model.Image != null)
                {
                    menuGroup.Image = await cloud.CloudinaryUploadImage(model.Image);
                }

                context.MenuGroups.Update(menuGroup);
                await context.SaveChangesAsync();

                var data = mapper.Map<MenuGroupV>(menuGroup);
                return new ApiResponse
                {
                    Message = "Cập nhật nhóm món ăn thành công",
                    StatusCode = 201,
                    Data = data,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in MenuGroupService/UpdateGroupMenuAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in MenuGroupService/UpdateGroupMenuAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
    }
}
