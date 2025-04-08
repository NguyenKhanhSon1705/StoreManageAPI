using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Config.Roles;
using StoreManageAPI.Context;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using System.Data;

namespace StoreManageAPI.DatabaseMigrations
{
    public class DatabaseMigrationsService
        (
            DataStore context,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            IConfiguration configuration ,
            ILogger<DatabaseMigrationsService> logger
        ) : IDatabaseMigrationsService
    {
        private readonly DataStore context = context;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;
        private readonly UserManager<User> userManager = userManager;
        private readonly IConfiguration configuration = configuration;
        private readonly ILogger<DatabaseMigrationsService> logger = logger;

        public async Task<ApiResponse> CheckConnectDatabaseAsync()
        {
            try
            {
                bool checkConnect = await context.Database.CanConnectAsync();
                if (!checkConnect)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        IsSuccess = false,
                        Message = "Không tìm thấy cơ sở dữ liệu hoặc không thể kết nối"
                    };
                }
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                    Message = "Kết nối cơ sở dữ liệu thành công"
                };
            }
            catch ( Exception ex )
            {
                logger.LogError($"There are errors in DatabaseMigrationsService/CheckConnectDatabaseAsync with message: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    Message = ex.Message,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<ApiResponse> CreateDatabaseAsync()
        {
            
            try
            {
                var databaseExists = await CheckConnectDatabaseAsync();
                if (databaseExists.IsSuccess)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        IsSuccess = false,
                        Message = "Cơ sở dữ liệu đã tồn tại"
                    };
                }

                bool createDatabase = await context.Database.EnsureCreatedAsync();

                if (!createDatabase)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        IsSuccess = false,
                        Message = "Khởi tạo cơ sở dữ liệu không thành công, Vui lòng thử lại"
                    };
                }

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                    Message = "Khởi tạo cơ sở dữ liệu thành công"
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are errors in DatabaseMigrationsService/CreateDatabaseAsync with message: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    Message = ex.Message,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<ApiResponse> DeleteDatabaseAsync(string secretKey)
        {
            
            try
            {
                if (secretKey == null || secretKey != configuration[Config.Config.AccessTokenSecret])
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        IsSuccess = false,
                        Message = "Khóa bí mật không hợp lệ"
                    };
                }

                var databaseExists = await CheckConnectDatabaseAsync();

                if (!databaseExists.IsSuccess)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        IsSuccess = false,
                        Message = "Cơ sở dữ liệu không tồn tại hoặc không thế kết nối"
                    };
                }

                bool deleted = await context.Database.EnsureDeletedAsync();

                if (!deleted)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        IsSuccess = false,
                        Message = "Xóa cơ sở dữ liệu không thành công"
                    };
                }
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                    Message = "Xóa cơ sở dữ liệu thành công"
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are errors in DatabaseMigrationsService/DeleteDatabaseAsync with message: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    Message = ex.Message,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<ApiResponse> ListTableInDataBaseAsync()
        {
            try
            {
                var databaseExists = await CheckConnectDatabaseAsync();
                if (!databaseExists.IsSuccess)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        IsSuccess = false,
                        Message = "Không thể kết nối được database hoặc không tồn tại"
                    };
                }

                var tableNames = new List<string>();
                using (var connection = context.Database.GetDbConnection())
                {
                    connection.Open();
                    var tables = connection.GetSchema("Tables");

                    foreach (DataRow row in tables.Rows)
                    {
                        tableNames.Add(row["TABLE_NAME"].ToString() ?? "");
                    }
                }
                return new ApiResponse
                {
                    Data = tableNames,
                    Message = "Danh sách các bảng trong database",
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are errors in DatabaseMigrationsService/ListTableInDataBaseAsync with message: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    Message = ex.Message,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<ApiResponse> MigrationsAddDatabaseAsync()
        {
            try
            {
                var databaseExists = await CheckConnectDatabaseAsync();
                if (!databaseExists.IsSuccess)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        IsSuccess = false,
                        Message = "Cơ sở dữ liệu không tồn tại hoặc không thế kết nối"
                    };
                }
                int tableCount = 0;

                using (var connection = context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    var tables = connection.GetSchema("Tables");
                    tableCount = tables.Rows.Count;
                }

                if(tableCount != 0)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        IsSuccess = false,
                        Message = "Đã tồn tại một số bảng trước đó không thế migrations"
                    };
                }

                await context.Database.MigrateAsync();
                return new ApiResponse
                {
                    Message = "Khởi tạo các bảng vào cơ sở dữ liệu thành công",
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are errors in DatabaseMigrationsService/MigrationsAddDatabaseAsync with message: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    Message = ex.Message,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<ApiResponse> SeedDataDefaultAsync()
        {
            try
            {
                var databaseExists = await CheckConnectDatabaseAsync();
                if (!databaseExists.IsSuccess)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        IsSuccess = false,
                        Message = "Không thể kết nối được database hoặc không tồn tại"
                    };
                }

                var rolenames = typeof(AppRoles).GetFields().ToList();
                foreach(var r in rolenames)
                {
                    var rolename = r.GetRawConstantValue()?.ToString() ?? "";
                    var rfound = await roleManager.FindByNameAsync(rolename);
                    if (rfound == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(rolename));
                    }
                }
                var rolenamesShop = typeof(AppRolesShop).GetFields().ToList();

                foreach (var r in rolenamesShop)
                {
                    var rolename = r.GetRawConstantValue()?.ToString() ?? "";
                    var rfound = await roleManager.FindByNameAsync(rolename);
                    if (rfound == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(rolename));
                    }
                }

                // admin@gmail.com // 12345

                var initUser = await userManager.FindByEmailAsync("admin@gmail.com");
                if (initUser != null)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        Message = "Đã tồn tại tài khoản khởi tạo"
                    };
                }
                initUser = new User
                {
                    UserName = "admin@gmail.com",
                    FullName = "Nguyễn Khánh Sơn",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(initUser , "12345");
                await userManager.AddToRoleAsync(initUser, Config.Roles.AppRoles.Administrator);

                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Khởi tạo vài trò thành công"
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are errors in DatabaseMigrationsService/MigrationsAddDatabaseAsync with message: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    Message = ex.Message,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
    }
}
