
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreManageAPI.Config;
using StoreManageAPI.Context;
using StoreManageAPI.Helpers.SendEmail;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Authentication;
using StoreManageAPI.ViewModels.UserManager;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static StoreManageAPI.Helpers.SendEmail.SendEmail;

namespace StoreManageAPI.Services.Auth
{
    public class AuthenService(
        ISendMail sendEmail,
        DataStore context,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthenService> logger,
        IJwtService jwtService,
        SignInManager<User> signInManager,
        UserManager<User> usermanage,
        RoleManager<IdentityRole> roleManager
        ) : IAuthenService
    {
        private readonly ISendMail sendEmail = sendEmail;
        private readonly DataStore context = context;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly IJwtService jwtService = jwtService;
        private readonly ILogger<AuthenService> logger = logger;
        private readonly SignInManager<User> signInManager = signInManager;
        private readonly UserManager<User> userManager = usermanage;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;

        private static string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString(); // Tạo mã 6 chữ số
        }


        public async Task<ApiResponse> RegisterAsync(RegisterV model)
        {
            try
            {
                if (await userManager.FindByEmailAsync(model.Email ?? "") != null)
                    return new ApiResponse() { StatusCode = 409, Message = "Email này đã được đăng ký" };

                string code = GenerateVerificationCode();
                var user = new User
                {
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    VerifiCode = code,
                    CodeExpireTime = DateTime.UtcNow.AddMinutes(2),
                    CreationDate = DateTime.UtcNow,
                    IsOwner = 1,
                    UserName = model.Email,
                };

                var result = await userManager.CreateAsync(user, model.Password ?? "");

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        logger.LogError($"There are an error for userManager: {item.Description} at {DateTime.UtcNow}");
                    }
                }

                string title = "Xác thực email của bạn";
                int time = Config.Config.CodeConfirmExpire;

                string html = FormatHTML.FormatConfirmEmailHTML(code, title, time);
                await sendEmail.SendEmailAsync(model.Email ?? "", title, html);

                return new ApiResponse()
                {
                    IsSuccess = true,
                    Message = "Đăng ký tài khoản thành công. Vui lòng xác thực tài khoản của bạn bao gồm 6 chữ số trong email",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"There are an error while handle register: ${ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse() { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<ApiResponse> ConfirmEmailAsync(ConfirmEmailV model)
        {
            var user = await userManager.FindByEmailAsync(model.Email ?? "");
            if (user == null)
            {
                return new ApiResponse()
                {
                    StatusCode = 404,
                    Message = "Không tìm thấy email này"
                };
            }

            if (user.VerifiCode != model.Code || user.CodeExpireTime < DateTime.UtcNow)
            {
                return new ApiResponse()
                {
                    StatusCode = 400,
                    Message = "Mã xác thực không hợp lệ"
                };
            }

            user.EmailConfirmed = true;
            user.CodeExpireTime = null;
            user.VerifiCode = null;
            await userManager.AddToRoleAsync(user, Config.Roles.AppRoles.Owner);

            await userManager.UpdateAsync(user);
            
            return new ApiResponse()
            {
                StatusCode = 200,
                Message = "Xác thực tài khoản thành công",
                IsSuccess = true
            };
        }

        public async Task<ApiResponse> ResendCodeConfirmEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse()
                {
                    StatusCode = 404,
                    Message = "Không tìm thấy email"
                };
            }

            string code = GenerateVerificationCode();

            user.CodeExpireTime = DateTime.UtcNow.AddMinutes(Config.Config.CodeConfirmExpire);
            user.VerifiCode = code;

            string title = "Xác thực email của bạn";
            int time = Config.Config.CodeConfirmExpire;

            string html = FormatHTML.FormatConfirmEmailHTML(code, title, time);
            await sendEmail.SendEmailAsync(email ?? "", title, html);

            await userManager.UpdateAsync(user);

            return new ApiResponse()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Gửi lại mã code thành công",
            };
        }

        public async Task<ApiResponse> ForgotPasswordAsync(string email)
        {

            var user = await userManager.FindByEmailAsync(email);
            if (user == null || user.EmailConfirmed)
            {
                return new ApiResponse()
                {
                    Message = "Không tìm thấy email này",
                    StatusCode = 404
                };
            }

            string code = GenerateVerificationCode();
            user.VerifiCode = code;
            user.CodeExpireTime = DateTime.UtcNow.AddMinutes(Config.Config.CodeConfirmExpire);

            string title = "Quên mật khẩu của bạn";
            int time = Config.Config.CodeConfirmExpire;

            string html = FormatHTML.FormatConfirmEmailHTML(code, title, time);
            await sendEmail.SendEmailAsync(email, title, html);

            await userManager.UpdateAsync(user);

            return new ApiResponse()
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Vui lòng kiểm tra email của bạn, Xác thực mã code",
            };
        }

        public async Task<ApiResponse> ConfirmChangePasswordAsync(ChangePasswordV model)
        {
            var user = await userManager.FindByEmailAsync(model.Email ?? "");

            if (user == null)
            {
                return new ApiResponse()
                {
                    StatusCode = 404,
                    Message = "Không tìm thấy email này"
                };
            }

            if (user.VerifiCode != model.code || user.CodeExpireTime < DateTime.UtcNow)
            {
                return new ApiResponse()
                {
                    StatusCode = 410,
                    Message = "Mã xác thực không hợp lệ"
                };
            }

            string ResetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await userManager.ResetPasswordAsync(user, ResetToken, model.Password ?? "");

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    logger.LogError($"There are an error from ConfirmChangePasswordAsync : {item.Description} at {DateTime.UtcNow} ");
                }

                return new ApiResponse()
                {
                    StatusCode = 500,
                    Message = "Có lỗi nào đó từ server"
                };
            }

            user.VerifiCode = null;
            user.CodeExpireTime = null;

            await userManager.UpdateAsync(user);

            return new ApiResponse()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Thay đổi mật khẩu thành công"
            };
        }

        public async Task<ApiResponse> LoginAsync(LogInV model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email ?? "");
                if (user == null)
                {
                    return new ApiResponse()
                    {
                        StatusCode = 404,
                        Message = "Tên người dùng hoặc mật khẩu không chính xác"
                    };
                }

                var result = await signInManager.PasswordSignInAsync(user, model.Password ?? "", false, false);
                if (!result.Succeeded)
                {
                    return new ApiResponse()
                    {
                        StatusCode = 404,
                        Message = "Tên người dùng hoặc mật khẩu không chính xác"
                    };
                }

                var tokenClaims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier , user.Id),
                new (ClaimTypes.Name , user.FullName ?? ""),
            };

                var userRoles = await userManager.GetRolesAsync(user);

                tokenClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var accessToken = jwtService.GenerateAccessToken(tokenClaims);
                var refreshToken = jwtService.GenerateRefreshToken(tokenClaims) ?? default;


                var SaveRefreshToken = new RefreshToken()
                {
                    JwtId = refreshToken?.TokenId,
                    UserId = user.Id,
                    TokenRefresh = refreshToken?.Token ?? "",
                    IsUsed = false,
                    IsRevoked = false,
                    IsMobile = false,
                    IssuedAt = DateTime.UtcNow,
                    ExpiredAt = refreshToken?.Expiration ?? DateTime.Now,
                };

                context.RefreshTokens.Add(SaveRefreshToken);
                await context.SaveChangesAsync();

                httpContextAccessor?.HttpContext?.Response.Cookies.Append(Config.Config.RefreshTokenCookiesName, refreshToken?.Token ?? "", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    // Domain = _configuration["API_DOMAIN"],
                    Expires = refreshToken?.Expiration
                });


                return new ApiResponse()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Đăng nhập thành công",
                    Data = new
                    {
                        accessToken = accessToken?.Token,
                        refreshToken = refreshToken?.Token
                    }
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred while logging in: {ex.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = "Lỗi máy chủ, vui lòng thử lại sau"
                };
            }
        }

        public async Task<ApiResponse> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var partsClaim = jwtService.GetClaimsFormToken(refreshToken);

                var refreshTokenId = partsClaim.FirstOrDefault(r => r.Type == JwtRegisteredClaimNames.Jti)?.Value;

                var userId = partsClaim.FirstOrDefault(r => r.Type == ClaimTypes.NameIdentifier)?.Value;

                if (!jwtService.CheckValidateToken(refreshToken, Config.Config.RefreshTokenSecret) || userId == null || refreshTokenId == null || userId == null)
                {
                    logger.LogError("Token invalid");
                    throw new InvalidOperationException("Token invalid");
                }

                var refreshTokenDb = await context.RefreshTokens.FirstOrDefaultAsync(r => r.JwtId == refreshTokenId);

                var partsClaimsFromDb = jwtService.GetClaimsFormToken(refreshTokenDb?.TokenRefresh);

                var isClaimValid = partsClaimsFromDb.All(c => partsClaim.Any(r => r.Type == c.Type && r.Value == c.Value));

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (refreshTokenDb.IsUsed || !isClaimValid || !jwtService.CheckValidateToken(refreshTokenDb.TokenRefresh, Config.Config.RefreshTokenSecret))
                {
                    logger.LogError("Token invalid");
                    throw new InvalidOperationException("Token invalid");
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                var newAccessTokenResponse = jwtService.GenerateAccessToken(partsClaimsFromDb);
                var newRefreshTokenResponse = jwtService.GenerateRefreshToken(partsClaimsFromDb);

                if (newRefreshTokenResponse == null || newAccessTokenResponse == null)
                {
                    logger.LogError("Cannot generate token");
                    throw new InvalidOperationException("Cannot generate token");
                }

                var RefreshToken = new RefreshToken
                {
                    JwtId = newRefreshTokenResponse.TokenId,
                    TokenRefresh = newRefreshTokenResponse.Token ?? "",
                    IssuedAt = DateTime.UtcNow,
                    ExpiredAt = newRefreshTokenResponse.Expiration,
                    IsUsed = false,
                    IsRevoked = false,
                    UserId = userId
                };

                context.RefreshTokens.Remove(refreshTokenDb);
                context.RefreshTokens.Add(RefreshToken);
                await context.SaveChangesAsync();

                httpContextAccessor?.HttpContext?.Response.Cookies.Append(Config.Config.RefreshTokenCookiesName, newRefreshTokenResponse.Token ?? "", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    // Domain = _configuration["API_DOMAIN"],
                    Expires = newRefreshTokenResponse.Expiration
                });

                return new ApiResponse
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Message = "Token đã được cập nhật",
                    Data = new
                    {
                        accessToken = newAccessTokenResponse.Token,
                        refreshToken = newRefreshTokenResponse.Token
                    }
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred while refreshing token: {ex.Message} at {DateTime.UtcNow}");
                if (ex is InvalidOperationException && ex.Message == "Token invalid")
                {
                    return new ApiResponse
                    {
                        StatusCode = 401,
                        IsSuccess = false,
                        Message = "Token không hợp lệ"
                    };
                }

                return new ApiResponse
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ApiResponse> LogoutAsync(string? accessToken, string? refreshToken)
        {
            try
            {
                var accessTokenClaims = new List<Claim>();
                var refreshTokenClaims = new List<Claim>();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    accessTokenClaims = jwtService.GetClaimsFormToken(accessToken);
                }
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    refreshTokenClaims = jwtService.GetClaimsFormToken(refreshToken);
                }

                var accessTokenId = accessTokenClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
                var refreshTokenId = refreshTokenClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                httpContextAccessor?.HttpContext?.Response.Cookies.Delete(Config.Config.RefreshTokenCookiesName);

                context.RefreshTokens.RemoveRange(context.RefreshTokens.Where(rt => rt.JwtId == refreshTokenId));
                await context.SaveChangesAsync();

                return new ApiResponse
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Message = "Đăng xuất thành công"
                };

            }
            catch (Exception e)
            {
                logger.LogError($"Error occurred while Authen/LogoutAsync out: {e.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = "Không thể đăng xuất, vui lòng thử lại sau hoặc liên hệ quản trị viên"
                };
            }
        }

        public async Task<ApiResponse> GetCurrentUserAsync(int shopId)
        {
            try
            {
                var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var user = await userManager.FindByIdAsync(userId ?? "");
                if (user == null)
                {
                    return new ApiResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        IsSuccess = false,
                        Message = "Không tìm thấy người dùng này"
                    };
                }

                var checkAcces = await context.ShopUser.AnyAsync(su => su.UserId == userId && su.ShopId == shopId);
                if (!checkAcces)
                {
                    return new ApiResponse
                    {
                        Message = "Bạn không thể thay đổi trạng thái",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }

                var shop = await context.Shop.FindAsync(shopId);



                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                    Data = new UserShopInfoV
                    {
                        FullName = user.FullName,
                        Email = user.Email,
                        Picture = user.Picture,

                        shopId = shop.Id,
                        ShopLogo = shop.ShopLogo,
                        ShopName = shop.ShopName,
                        IsActive = shop.IsActive,
                    }
                };

            }
            catch (Exception e)
            {
                logger.LogError($"Error occurred while Authen/GetCurrentUser out: {e.Message} at {DateTime.UtcNow}");
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    IsSuccess = false,
                    Message = "Error occurred while Authen/GetCurrentUser"
                };
            }


            
        }
    }
}
