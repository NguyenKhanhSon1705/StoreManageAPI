using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Context;
using StoreManageAPI.Helpers.Paging;
using StoreManageAPI.Mddleware;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.UserManager;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StoreManageAPI.Services.UserManager
{
    public class UserService
        (
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper,
        IHttpContextAccessor httpcontextAccessor,
        ILogger<UserService> logger,
        CloudinaryMiddle cloud,
        DataStore context


        ) : IUserService
    {
        private readonly UserManager<User> userManager = userManager;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;
        private readonly IMapper mapper = mapper;
        private readonly IHttpContextAccessor httpcontextAccessor = httpcontextAccessor;
        private readonly ILogger<UserService> logger = logger;
        private readonly CloudinaryMiddle cloud = cloud;
        private readonly DataStore context = context;

        public int DEFAULT_PAGE_SIZE = 10;
        public int DEFAULT_PAGE_INDEX = 1;
        public int DEFAULT_SEARCH_RESULT = 10;

        public async Task<ApiResponse> CreateUserAsync(CreateUserV model)
        {
            try
            {
                if (await userManager.FindByEmailAsync(model.Email ?? "") != null)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Email này đã tồn tại",
                    };
                }

                var idCurrentUser = httpcontextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                
                if (await userManager.FindByIdAsync(idCurrentUser ?? "") == null)
                {
                     return new ApiResponse
                     {
                         StatusCode = StatusCodes.Status404NotFound,
                         Message = "Tài khoản đang đăng nhập không hợp lệ!",
                     };
                }
                var shop = await context.Shop.FindAsync(model.IdShop);

                if (shop == null)
                {
                    return new ApiResponse
                    {
                        Message = "Thông tin cửa hàng không tồn tại",
                        StatusCode = 404
                    };
                }

                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    UserName = model.Email,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    ManagerID = idCurrentUser,
                    CreationDate = DateTime.UtcNow,
                    BirthDay = model.BirthDay,
                    Gender = model.Gender,
                    EmailConfirmed = true
                };

                var newUser = await userManager.CreateAsync(user, model.PhoneNumber ?? "");

                var shopUser = new ShopUser
                {
                    ShopId = shop.Id,
                    User = user
                };
                await context.ShopUser.AddAsync(shopUser);

                if (!newUser.Succeeded)
                {
                    foreach(var e in newUser.Errors)
                    {
                        return new ApiResponse
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Message = e.Description,
                        };
                    }
                }

                var role = await roleManager.FindByIdAsync(model.RoleId ?? "");

                if(role == null)
                {
                    return new ApiResponse
                    {
                        Message = "Vai trò này không tồn tại",
                        StatusCode = 404
                    };
                }

                await userManager.AddToRoleAsync(user, role.Name ?? "");
                
                var userRreturn = mapper.Map<UserInfoV>(user);
                userRreturn.Roles = role.Name;
                userRreturn.RoleId = role.Id;

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Thêm người dùng thành công",
                    IsSuccess = true,
                    Data = userRreturn
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in UserService/CreateUser: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = 400,
                    Message = $"There are something error in UserService/CreateUser: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> UpdateUserAsync(UpdateUserV model)
        {
            try
            {
                var user = await userManager.FindByIdAsync(model.Id ?? "");
                if (user == null)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Không tìm thấy user",
                    };
                }

                var idCurrentUser = httpcontextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (model.Picture != null)
                {
                    user.Picture = await cloud.CloudinaryUploadImage(model.Picture);
                }

                user.FullName = model.FullName ?? user.FullName;
                user.BirthDay = model.BirthDay ?? user.BirthDay;
                user.PhoneNumber = model.PhoneNumber;
                user.Gender = model.Gender;
                user.Address = model.Address;
                
                var role = await roleManager.FindByIdAsync(model.RoleId ?? "");

                if (role == null)
                {
                    return new ApiResponse
                    {
                        StatusCode = 200,
                        Message = "Vai trò không tồn tại",
                    };
                }
                var currentRole = await userManager.GetRolesAsync(user);

                await userManager.RemoveFromRolesAsync(user,currentRole);

                await userManager.AddToRoleAsync(user,role.Name ?? "");

                var result =  await userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        logger.LogError($"There are something error in UserService/UpdateUserAsync: {item} at {DateTime.UtcNow}");
                    }
                }

                return new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Cập nhật thành công" , 
                    Data = mapper.Map<CreateUserV>(user),
                    IsSuccess = true
                    
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in UserService/UpdateUserAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = 400,
                    Message = $"There are something error in UserService/UpdateUserAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }
        public async Task<ApiResponse> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                var managerName = await userManager.FindByIdAsync(user?.ManagerID ?? "");
                if (user == null)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy người dùng hoặc người dùng đã bị khóa"
                    };
                }

                var userInfo = mapper.Map<UserInfoV>(user);

                var roles = await userManager.GetRolesAsync(user);
                var role = await roleManager.FindByNameAsync(roles.First());
                userInfo.RoleId = role.Id;
                userInfo.Roles = role.Name;
                userInfo.ManagerName = managerName?.FullName ?? managerName?.Email;

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                    Message = "Lấy thông tin người dùng thành công",
                    Data = userInfo
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in UserService/GetUserById: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in UserService/GetUserById: {ex.Message} at {DateTime.UtcNow}",
                };
            }

        }

        public async Task<ApiResponse> LockUserAsync(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null || user.IsLock == true)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        IsSuccess = false,
                        Message = "Người dùng không tồn tại hoặc đã bị khóa"
                    };
                }

                var idCurrentUser = httpcontextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                /*if (idCurrentUser == null)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        IsSuccess = false,
                        Message = "Không tìm thấy thông tin người khóa tài khoản"
                    };
                }*/

                user.IsLock = true;
                user.LockAtDate = DateTime.UtcNow;
                user.LockByUser = idCurrentUser;
                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        IsSuccess = false,
                        Message = "Người dùng không tồn tại hoặc đã bị xóa"
                    };
                }
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                    Message = "Khóa tài khoản thành công",
                    Data = user.Id

                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in UserService/LockUserAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in UserService/LockUserAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> UnLockUserAsync(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null || user.IsLock == false )
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        IsSuccess = false,
                        Message = "Người dùng không tồn tại hoặc không bị khóa"
                    };
                }
                var idCurrentUser = httpcontextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                user.IsLock = false;
                user.LockAtDate = DateTime.UtcNow;
                user.LockByUser = idCurrentUser;
                var resutl = await userManager.UpdateAsync(user);
                if (!resutl.Succeeded)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        IsSuccess = false,
                        Message = "Người dùng không tồn tại hoặc đã bị xóa"
                    };
                }
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                    Message = "Mở khóa tài khoản thành công",
                    Data = user.Id
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in UserService/UnLockUserAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in UserService/UnLockUserAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> SearchUsersAsync(string? searchKey, int? limit)
        {
            try
            {
                var quantityResult = limit ?? 10;
                var query = userManager.Users.AsQueryable();
                if (!string.IsNullOrEmpty(searchKey))
                {
                    query = query.Where(r => r.FullName != null && r.FullName.Contains(searchKey));
                }
                query = query.Where(u => u.IsLock == false || u.IsLock == null).Take(quantityResult);

                var result = mapper.Map<IList<UserInfoV>>(await query.ToListAsync());
                
                var quantity = result.Count();

                return new ApiResponse
                {
                    Message = $"Tìm thấy {quantity} kết quả",
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                    Data = new
                    {
                        Users = result,
                        Total = quantity,
                    }
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in UserService/SearchUsersAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in UserService/SearchUsersAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> GetAllUserAsync(int? PageIndex, int? limit, int shop_id = 5)
        {
            try
            {
                //var query = context.ShopUser.Include(inc => inc.User).Where(w => w.ShopId == shop_id).AsQueryable();

                var query = userManager.Users.AsQueryable();
                
                
                query = query.Where(u => u.IsLock == false || u.IsLock == null);
                query = query.OrderByDescending(u => u.CreationDate);

                var pageIndex = PageIndex ?? DEFAULT_PAGE_INDEX;
                var pageSize = limit ?? DEFAULT_PAGE_SIZE;

                var result = await Paging<User, User>.CreateAsync(query, pageIndex, pageSize);
                var userList = new Paging<UserInfoV, UserInfoV>(mapper.Map<List<UserInfoV>>(result.Items), result.TotalCount, result.PageIndex, result.PageSize);
                

                foreach (var user in result.Items)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    var userVM = userList.Items.FirstOrDefault(u => u.Id == user.Id);
                    if (userVM != null)
                    {
                        userVM.Roles = roles.FirstOrDefault();
                    }
                }
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                    Message = "Lấy danh sách người dùng thành công",
                    Data = userList
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in UserService/GetAllUserAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in UserService/GetAllUserAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> GetAllLockUsersAsync(int? PageIndex, int? limit)
        {
            try
            {
                var query = userManager.Users.AsQueryable();
                query = query.Where(u => u.IsLock == true);
                query = query.OrderByDescending(u => u.LockAtDate);
                var pageIndex = PageIndex ?? DEFAULT_PAGE_INDEX;
                var pageSize = limit ?? DEFAULT_PAGE_SIZE;

                var result = await Paging<User, User>.CreateAsync(query, pageIndex, pageSize);
                var userList = new Paging<UserInfoV, UserInfoV>(mapper.Map<List<UserInfoV>>(result.Items), result.TotalCount, result.PageIndex, result.PageSize);

                foreach (var user in result.Items)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    var userVM = userList.Items.FirstOrDefault(u => u.Id == user.Id);
                    if (userVM != null)
                    {
                        userVM.Roles = roles.FirstOrDefault();
                    }
                }
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                    Message = "Lấy danh sách người dùng bị khóa thành công",
                    Data = userList
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in UserService/GetAllLockUsersAsync: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in UserService/GetAllLockUsersAsync: {ex.Message} at {DateTime.UtcNow}",
                };
            }
        }

        public async Task<ApiResponse> GetUserOfTreeAsync()
        {
            var parentId = httpcontextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (parentId == null)
            {
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy người dùng",
                    IsSuccess = false
                };
            }

            var users = await userManager.Users.Where(u => u.ManagerID == parentId && u.IsLock != true).ToListAsync();

            // Tạo danh sách UserInfoV với role
            var result = new List<UserInfoV>();

            foreach (var user in users)
            {
                // Lấy các vai trò của user
                var roles = await userManager.GetRolesAsync(user);

                // Mapping user với thông tin và role
                var userInfo = mapper.Map<UserInfoV>(user);
                userInfo.Roles = roles.FirstOrDefault(); 

                result.Add(userInfo);
            }
            return new ApiResponse
            {
                Message = "Danh sách user",
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Data = result
            };
        }

        public async Task<ApiResponse> GetUserOfTreeByIdAsync(string userId)
        {
            if (userId == null)
            {
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy người dùng",
                    IsSuccess = false
                };
            }
            var users = await userManager.Users.Where(u => u.ManagerID == userId && u.IsLock != true).ToListAsync();

            // Tạo danh sách UserInfoV với role
            var result = new List<UserInfoV>();

            foreach (var user in users)
            {
                // Lấy các vai trò của user
                var roles = await userManager.GetRolesAsync(user);

                // Mapping user với thông tin và role
                var userInfo = mapper.Map<UserInfoV>(user);
                userInfo.Roles = roles.FirstOrDefault();

                result.Add(userInfo);
            }
            return new ApiResponse
            {
                Message = "Danh sách user",
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Data = result
            };
        }
    
        
    }
}
