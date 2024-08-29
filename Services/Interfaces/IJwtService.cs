using StoreManageAPI.ViewModels.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StoreManageAPI.Services.Interfaces
{
    public interface IJwtService
    {
        TokenResponse? GenerateAccessToken(List<Claim> authClaims);
        TokenResponse? GenerateRefreshToken(List<Claim> authClaims);

        public List<Claim> GetClaimsFormToken(string? token);

        public bool CheckValidateToken(string? token , dynamic op);

       
        


    }
}
