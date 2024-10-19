using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManageAPI.Config;
using StoreManageAPI.Config.Roles;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.UserManager;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace StoreManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Quản lý người dùng")]
    [Authorize]
    public class UsersController
        (
        IUserService userService
        ) : ControllerBase
    {
        private readonly IUserService userService = userService;

        [HttpPost("create-user")]
        [Authorize(Roles = AppRoles.Owner + "," + AppRoles.Manager)]
        [SwaggerOperation( "Thêm người dùng thủ công" ,"Thêm tài khoản người dùng do một người có chức vụ lớn hơn hoặc có quyền thêm")]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserV model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Dữ liệu không hợp lệ",
                    Data = ModelState.Values.ToList()
                });
            }

            var result = await userService.CreateUserAsync(model);
            if(!result.IsSuccess)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Dữ liệu không hợp lệ",
                    Data = result.Message
                });
            }
            return StatusCode(result.StatusCode , result);
        }

        [HttpPut("update-user")]
        [SwaggerOperation("Thêm người dùng thủ công", "Thêm tài khoản người dùng do một người có chức vụ lớn hơn hoặc có quyền thêm")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserV model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await userService.UpdateUserAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return StatusCode(result.StatusCode , result);
        }

        [HttpGet("get-user-id")]
        [Authorize(Roles = AppRoles.Owner + "," + AppRoles.Manager)]
        [SwaggerOperation("Chi tiết người dùng" , "Tìm thông tin người dùng theo ID")]
        public async Task<IActionResult> GetUserById([FromQuery] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Dữ liệu không hợp lệ",
                    Data = ModelState.Values.ToList()
                });
            }
            var result = await userService.GetUserByIdAsync(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return StatusCode(result.StatusCode , result);
        }

        [HttpPut("lock-user")]
        [Authorize(Roles = AppRoles.Owner + "," + AppRoles.Manager)]
        [SwaggerOperation("Khóa người dùng theo ID" , "Khóa tạm thời một tài khoản người dùng theo ID")]
        public async Task<IActionResult> LockUser([FromBody] ParamsIdV model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse 
                {
                    StatusCode= StatusCodes.Status400BadRequest,
                    Message = "Dữ liệu không hợp lệ",
                    Data= ModelState.Values.ToList()
                });
            }
            var result = await userService.LockUserAsync(model.Id ?? "");
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return StatusCode(result.StatusCode , result);
        }
        [HttpPut("unlock-user")]
        [Authorize(Roles = AppRoles.Owner + "," + AppRoles.Manager)]
        [SwaggerOperation("Mở khóa người dùng theo ID", "Mở khóa tạm thời một tài khoản người dùng theo ID")]
        public async Task<IActionResult> UnLockUser([FromBody] ParamsIdV model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Dữ liệu không hợp lệ",
                    Data = ModelState.Values.ToList()
                });
            }
            var result = await userService.UnLockUserAsync(model.Id ?? "");
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("search-user")]
        [Authorize(Roles = AppRoles.Owner + "," + AppRoles.Manager)]
        [SwaggerOperation("Tìm kiếm" , "Tìm kiếm thông tin người dùng với trường họ tên")]
        public async Task<IActionResult> SearchUsers([FromQuery] string search , [FromQuery] int limit = 10)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Dữ liệu không hợp lệ",
                    Data = ModelState.Values.ToList()
                });
            }
            var result = await userService.SearchUsersAsync(search , limit );

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return StatusCode(result.StatusCode, result);   
        }

        [HttpGet("get-user-all")]
        [SwaggerOperation("Lấy tất cả thông tin người dùng", "Lấy tất cả thông tin người dùng")]
        [Authorize(Roles = AppRoles.Owner)]
        public async Task<IActionResult> GetAllUser([FromQuery] int? PageIndex = 1,[FromQuery] int? limit = 10)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Dữ liệu không hợp lệ",
                    Data = ModelState.Values.ToList()
                });
            }
            var result = await userService.GetAllUserAsync(PageIndex, limit);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("get-user-all-islock")]
        [Authorize(Roles = AppRoles.Owner)]
        [SwaggerOperation("Lấy tất cả thông tin người dùng bị khóa", "Lấy tất cả thông tin người dùng bị khóa")]
        public async Task<IActionResult> GetAllLockUsers([FromQuery] int? PageIndex = 1, [FromQuery] int? limit = 10)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Dữ liệu không hợp lệ",
                    Data = ModelState.Values.ToList()
                });
            }
            var result  = await userService.GetAllLockUsersAsync(PageIndex, limit);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        
        [HttpGet("get-user-tree")]
        [Authorize(Roles = AppRoles.Owner + "," + AppRoles.Manager)]
        [SwaggerOperation("Lấy tất cả thông tin người dùng bị khóa", "Lấy tất cả thông tin người dùng bị khóa")]
        public async Task<IActionResult> GetUserOfTree()
        {
            var result = await userService.GetUserOfTreeAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get-user-tree-by-id")]
        [Authorize(Roles = AppRoles.Owner + "," + AppRoles.Manager)]
        [SwaggerOperation("Lấy tất cả thông tin người dùng bị khóa", "Lấy tất cả thông tin người dùng bị khóa")]
        public async Task<IActionResult> GetUserOfTreeById(string id)
        {
            var result = await userService.GetUserOfTreeByIdAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
