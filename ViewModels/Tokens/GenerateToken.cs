using Microsoft.IdentityModel.Tokens;
using StoreManageAPI.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StoreManageAPI.Functions.Tokens
{
    public class Token
        (
            IConfiguration configuration
        )
    {
        private readonly IConfiguration configuration = configuration;
        
        public string GenerateAccessToken(string userId, IList<string> roles)
        {
            var secret = Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"] ?? "");
            var issuer = configuration["JWT:Issuer"];
            var audience = configuration["JWT:Audience"];
               
            var expiration = DateTime.UtcNow.AddMinutes(30); // Thời gian hết hạn là 30 phút kể từ bây giờ
            var expTimestamp = new DateTimeOffset(expiration).ToUnixTimeSeconds().ToString(); // Chuyển đổi thành Unix timestamp

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, userId),
                new (JwtRegisteredClaimNames.Exp , expTimestamp),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var secretKey = new SymmetricSecurityKey(secret);
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                issuer,
                audience,
                claims,
                expiration,
                signingCredentials: credentials
                );

            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken;
              
        }

        public string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var r = RandomNumberGenerator.Create())
            {
                r.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
    }
}
