using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Context;
using StoreManageAPI.Mddleware;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Shopes;
using StoreManageAPI.ViewModels.Stores;
using System.Security.Claims;

namespace StoreManageAPI.Services.Store
{
    public class ShopService 
        (
         DataStore context,
         ILogger<ShopService> logger,
         UserManager<User> userManager,
         IHttpContextAccessor httpContext,
         IMapper mapper,
         CloudinaryMiddle cloud
        ) : IShopSerVice
    {
        private readonly DataStore context = context;
        private readonly ILogger<ShopService> logger = logger;
        private readonly UserManager<User> userManager = userManager;
        private readonly IHttpContextAccessor httpContext = httpContext;
        private readonly IMapper mapper = mapper;
        private readonly CloudinaryMiddle cloud = cloud;
        public async Task<ApiResponse> CreateShopAsync(ShopV model)
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
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                var newShop = new Models.Shop()
                {
                    ShopName = model.ShopName,
                    ShopAddress = model.ShopAddress,
                    ShopPhone = model.ShopPhone,
                    IsActive = false,
                    LockStore = false,
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow,
                };

                if(model.ShopLogo != null)
                {
                    newShop.ShopLogo = await cloud.CloudinaryUploadImage(model.ShopLogo); 
                }

                context.Shop.Add(newShop);

                context.ShopUser.Add(new ShopUser { Shop = newShop, UserId = userId });

                await context.SaveChangesAsync();

                return new ApiResponse 
                {   
                    Message = "Tạo cửa hàng thành công",
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                    Data = mapper.Map<GetShopV>(newShop)
                };

            }
            catch ( Exception ex )
            {
                logger.LogError($"There are something error in ShopService/CreateStoreName: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in ShopService/CreateStoreName: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> DeleteShopAsync(int id , string password)
        {
            try
            {
                var userOnwerId = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var user = await userManager.FindByIdAsync(userOnwerId ?? "");

                if (user.IsOwner !=1 ) {
                    return new ApiResponse
                    {
                        Message = "Vui lòng liên hệ chủ cửa hàng để thực hiện chức năng này",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }

                var checkAcces = await context.ShopUser.AnyAsync(su => su.UserId == userOnwerId && su.ShopId == id);
                if (!checkAcces)
                {
                    return new ApiResponse
                    {
                        Message = "Bạn không thể thay đổi trạng thái",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }

                var userOnwer = await userManager.FindByIdAsync(userOnwerId ?? "");

                if (userOnwer == null)
                {
                    return new ApiResponse
                    {
                        Message = "Bạn không thể thay đổi trạng thái",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }

                var checkPassword = await userManager.CheckPasswordAsync(userOnwer, password);
                if (!checkPassword)
                {
                    return new ApiResponse
                    {
                        Message = "Tài khoản hoặc mật khẩu không hợp lệ",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }

                var curShop = await context.Shop.FindAsync(id);
                if (curShop == null)
                {
                    return new ApiResponse
                    {
                        Message = "Cửa hàng không tồn tại",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }

                context.Shop.Remove(curShop);
                await context.SaveChangesAsync();
                return new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Xóa cửa hàng",
                    IsSuccess = true,
                    Data = curShop
                };
            }
            catch ( Exception ex )
            {
                logger.LogError($"There are something error in ShopService/UpdateStoreName: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in ShopService/UpdateStoreName: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> GetListShopAsync()
        {
            try
            {
                var userId = httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if(userId == null )
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy người dùng hiện tại",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                var listShop = await context.ShopUser
                    .Include(x => x.Shop)
                    .Where(w => w.UserId == userId)
                    .Select(s => new GetShopV
                    {
                        Id = s.Shop.Id,
                        ShopName = s.Shop.ShopName,
                        ShopLogo = s.Shop.ShopLogo,
                        ShopPhone = s.Shop.ShopPhone,
                        ShopAddress = s.Shop.ShopAddress,
                        IsActive = s.Shop.IsActive
                    }).ToListAsync();

                
                return new ApiResponse
                {
                    Message = "Thông tin cửa hàng",
                    Data = listShop,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in ShopService/GetListShopAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in ShopService/GetListShopAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> GetShopByIdAsync(int id)
        {
            try
            {
                var userOnwerId = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var checkAcces = await context.ShopUser.AnyAsync(su => su.UserId == userOnwerId && su.ShopId == id);
                if (!checkAcces)
                {
                    return new ApiResponse
                    {
                        Message = "Bạn không thể thay đổi trạng thái",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }


                var curShop = await context.Shop.FindAsync(id);

                if (curShop == null)
                {
                    return new ApiResponse
                    {
                        Message = "Không tìm thấy thông tin cửa hàng",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }

                return new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Thông tin cửa hàng",
                    IsSuccess = true,
                    Data = curShop
                };


            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in ShopService/IsActiveShop: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in ShopService/IsActiveShop: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> IsActiveShopAsync(int id)
        {
            try
            {
                var userOnwerId = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var checkAcces = await context.ShopUser.AnyAsync(su => su.UserId == userOnwerId && su.ShopId == id);
                if (!checkAcces)
                {
                    return new ApiResponse
                    {
                        Message = "Bạn không thể thay đổi trạng thái",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }

                var curShop = await context.Shop.FindAsync(id);
                if (curShop == null)
                {
                    return new ApiResponse
                    {
                        StatusCode= StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy thông tin cửa hàng",
                    };
                }
                curShop.IsActive = !curShop.IsActive;
                context.Update(curShop);
                await context.SaveChangesAsync();
                return new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Cập nhật trạng thái cửa hàng thành công",
                    IsSuccess= true,
                    Data = curShop.IsActive
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in ShopService/IsActiveShop: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in ShopService/IsActiveShop: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> UpdateShopAsync(ShopV model)
        {
            try
            {
                var curShop = await context.Shop.FirstOrDefaultAsync(s => s.Id == model.Id);

                if (model.ShopLogo != null)
                {
                    curShop.ShopLogo = await cloud.CloudinaryUploadImage(model.ShopLogo);
                }

                //if (model.ShopLogo != null && model.ShopLogo.Length > 0)
                //{
                //    curShop.ShopLogo = await cloud.CloudinaryUploadImage(model.ShopLogo);
                //}

                curShop.ShopName = model.ShopName ?? curShop.ShopName;
                curShop.ShopPhone = model.ShopPhone ?? curShop.ShopPhone;
                curShop.ShopAddress = model.ShopAddress ?? curShop.ShopAddress;
                curShop.UpdateAt = DateTime.Now;

                context.Shop.Update(curShop);
                await context.SaveChangesAsync();

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Cập nhật cửa hàng thành công",
                    Data = curShop,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in ShopService/UpdateStoreAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in ShopService/UpdateStoreAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
    
        
    
    }
}
