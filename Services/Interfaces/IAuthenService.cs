using StoreManageAPI.ModelReturnData;
using StoreManageAPI.ViewModels.Authentication;

namespace StoreManageAPI.Services.Interfaces
{
    public interface IAuthenService
    {
        public Task<ApiResponse> RegisterAsync(RegisterV register);
        public Task<ApiResponse> ConfirmEmailAsync(ConfirmEmailV model);
        public Task<ApiResponse> ResendCodeConfirmEmailAsync(string email);
        public Task<ApiResponse> ForgotPasswordAsync(string email);
        public Task<ApiResponse> ConfirmChangePasswordAsync(ChangePasswordV model);
        public Task<ApiResponse> LoginAsync(LogInV logIn);
        public Task<ApiResponse> RefreshTokenAsync(string refreshToken);
        public Task<ApiResponse> LogoutAsync(string? accessToken , string? refreshToken);
    }
}
