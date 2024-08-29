using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManageAPI.Config;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Authentication;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using UAParser;

namespace StoreManageAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Xác thực người dùng")]
    public class AuthenController(
        IAuthenService authService,
        ILogger<AuthenController> logger
        ) : ControllerBase
    {
        private readonly IAuthenService authService = authService;
        private readonly ILogger<AuthenController> logger = logger;

        [HttpPost("/register")]
        [SwaggerOperation("Đăng ký" , "Email, số điện thoại, mật khẩu và yêu cầu xác thức email của người dùng")]
        public async Task<IActionResult> Register([FromBody] RegisterV model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new ApiResponse()
                    {
                        StatusCode = 400,
                        Message = "Thông tin đăng nhập không hợp lệ"
                    }
                );
            }
            try
            {
                var result = await authService.RegisterAsync(model);
                if (!result.IsSuccess)
                {
                    return StatusCode(result.StatusCode , result);
                }
                return Ok(result);
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error occurred while Register in: {ex.Message} at {DateTime.UtcNow}");
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "Error occurred while Register in, please try again later or contact administrator"
                });
            }
        }

        [HttpPost("/confirmemail")]
        [SwaggerOperation("Nhập mã xác thực", "Mã code được gửi về email của người dùng trong 2 phút để xác thực email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailV model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new ApiResponse()
                    {
                        StatusCode = 400,
                        Message = "Thông tin dữ liệu không hợp lệ"
                    }
                );
            }
            try
            {
                var resutl = await authService.ConfirmEmailAsync(model);
                if (!resutl.IsSuccess)
                {
                    return StatusCode(resutl.StatusCode , resutl);
                }
                return Ok(resutl);
            }
            catch (Exception ex)
            { 
                logger.LogError($"Error occurred while ConfirmEmail in: {ex.Message} at {DateTime.UtcNow}");
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "Error occurred while ConfirmEmail in, please try again later or contact administrator"
                });
            }

        }

        [HttpPost("/resendcodeconfirmemail")]
        [SwaggerOperation("Gửi lại mã code" , "Khi mã code hết hạn người dùng yêu cầu cấp lại mã code gửi về email.")]
        public async Task<IActionResult> ResendCodeConfirmEmail([FromBody][EmailAddress]string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new ApiResponse()
                    {
                        StatusCode = 400,
                        Message = "Thông tin dữ liệu không hợp lệ"
                    }
                );
            }
            try
            {
                var result = await authService.ResendCodeConfirmEmailAsync(email);
                if (!result.IsSuccess)
                {
                    return StatusCode(result.StatusCode , result);
                }
                return Ok(result);
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error occurred while ResendCodeConfirmEmail in: {ex.Message} at {DateTime.UtcNow}");
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "Error occurred while ResendCodeConfirmEmail in, please try again later or contact administrator"
                });
            }
        }

        [HttpPost("/forgotpassword")]
        [SwaggerOperation("Yêu cầu quên mật khẩu" , "Gửi một yêu cầu về email của bạn để lấy lại mật khẩu")]
        public async Task<IActionResult> ForgotPassword([FromBody][EmailAddress]string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                   new ApiResponse()
                   {
                       StatusCode = 400,
                       Message = "Thông tin dữ liệu không hợp lệ"
                   }
                );
            }
            try
            {
                var result = await authService.ForgotPasswordAsync(email);
                if (!result.IsSuccess)
                {
                    return StatusCode(result.StatusCode , result);
                }
                    return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred while ForgotPassword in: {ex.Message} at {DateTime.UtcNow}");
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "Error occurred while ForgotPassword in, please try again later or contact administrator"
                });
            }
        }

        [HttpPost("/confirmChangePassword")]
        [SwaggerOperation("Thay đổi mật khẩu" , "Lấy mã code được gửi trong email và thay đổi mật khẩu")]
        public async Task<IActionResult> ConfirmChangePassword([FromBody] ChangePasswordV model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                   new ApiResponse()
                   {
                       StatusCode = 400,
                       Message = "Thông tin dữ liệu không hợp lệ"
                   }
                );
            }
            try
            {
                var result = await authService.ConfirmChangePasswordAsync(model);
                if(!result.IsSuccess)
                {
                    return StatusCode(result.StatusCode , result);
                }  
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred while ConfirmChangePassword in: {ex.Message} at {DateTime.UtcNow}");
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "Error occurred while ConfirmChangePassword in, please try again later or contact administrator"
                });
            }
        }
        [HttpPost("/Login")]
        [SwaggerOperation("Đăng nhập" , "Đăng nhập và hệ thông gửi về mã token và refreshtoken")]
        public async Task<IActionResult> LogIn([FromBody]LogInV model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                   new ApiResponse()
                   {
                       StatusCode = 400,
                       Message = "Thông tin dữ liệu không hợp lệ"
                   }
                );
            }
            try
            {
                var result = await authService.LoginAsync(model);
                if(! result.IsSuccess)
                {
                    return StatusCode(result.StatusCode , result);
                } 
                return Ok(result);
            }catch (Exception ex)
            {
                logger.LogError($"Error occurred while LogIn in: {ex.Message} at {DateTime.UtcNow}");
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "Error occurred while LogIn in, please try again later or contact administrator"
                });
            }
        }

        [HttpPost("/refreshToken")]
        [SwaggerOperation("Cấp lại access token", "Lấy access-token khi token đã hết hạn")]
        public async Task<IActionResult> RefreshToken()
        {
            string? refreshToken = Request.Cookies["refreshToken"];
                logger.LogError("sss " + refreshToken);
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(
                   new ApiResponse
                   {
                       StatusCode = 401,
                       Message = "You are not authorized, please login to get access"
                   }
               );
            }
            try
            {
                var data = await authService.RefreshTokenAsync(refreshToken);
                if (!data.IsSuccess)
                {
                    return StatusCode(data.StatusCode, data);
                }
                return Ok(data);

            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred while RefreshToken token: {ex.Message} at {DateTime.UtcNow}");
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "Error occurred while RefreshToken token, please try again later or contact administrator"
                });
            }
        }

        [HttpPost("/logout")]
        [SwaggerOperation("Đăng xuất" , "Đăng xuất")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var accessToken = await Request.HttpContext.GetTokenAsync("access_token");
            var refreshToken = Request.Cookies[ConfigAppSetting.RefreshTokenCookiesName];

            var data = await authService.LogoutAsync(accessToken, refreshToken);
            return StatusCode(data.StatusCode, data);
        }

        [HttpGet("test")]
        [Authorize]
        public IActionResult test()
        {
            var userAgentString = HttpContext.Request.Headers["User-Agent"].ToString();
            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgentString);
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var deviceInfo = new
            {
                Device = clientInfo.Device.Family,
                OS = clientInfo.OS.Family,
                Browser = clientInfo.UA.Family,
                IPAddress = ipAddress
            };

            return Ok(deviceInfo);

        }
    }
}