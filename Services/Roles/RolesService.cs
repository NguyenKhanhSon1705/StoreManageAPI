using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreManageAPI.Config.Roles;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Interfaces;
using System.Linq;

namespace StoreManageAPI.Services.Roles
{
    public class RolesService
        (
            RoleManager<IdentityRole> roleManager,
            ILogger<RolesService> logger
        )
        : IRolesService
    {
        private readonly RoleManager<IdentityRole> roleManager = roleManager;
        private readonly ILogger<RolesService> logger = logger;
        public async Task<ApiResponse> GetListRoleShop()
        {
            try
            {
                List<string> roleDis = new List<string>();

                var rolenamesShop = typeof(AppRoles).GetFields().ToList();
                foreach ( var rolename in rolenamesShop)
                {
                    string r = rolename.GetRawConstantValue().ToString() ?? "";
                    roleDis.Add( r );
                }
                // Lấy toàn bộ role từ database
                var listRole = await roleManager.Roles
                    .Select(role => new
                    {
                        role.Name,
                        role.Id
                    })
                    .ToListAsync();  // Lấy dữ liệu từ database

                // Lọc ra các role không có trong danh sách roleDis
                var filteredRoles = listRole
                    .Where(role => !roleDis.Contains(role.Name ?? ""))  // Loại bỏ role có tên trong roleDis
                    .ToList();

                return new ApiResponse
                {
                    Data = filteredRoles,
                    Message = "Danh sách vai trò cửa hàng",
                    IsSuccess = true,
                    StatusCode = 200
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"There are something error in RolesService/GetListRoleShop: {ex.Message} at {DateTime.UtcNow}");

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"There are something error in RolesService/GetListRoleShop: {ex.Message} at {DateTime.UtcNow}",
                };
            }

        }
    }
}
