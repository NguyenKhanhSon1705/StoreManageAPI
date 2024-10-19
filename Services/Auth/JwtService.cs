using Microsoft.IdentityModel.Tokens;
using MimeKit.Tnef;
using Newtonsoft.Json.Linq;
using StoreManageAPI.Config;
using StoreManageAPI.Functions.Tokens;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Authentication;
using StoreManageAPI.ViewModels.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreManageAPI.Services.Auth
{
    public class JwtService
        (
            IConfiguration configuration,
            ILogger<JwtService> logger
        ) : IJwtService
    {
        private readonly IConfiguration configuration = configuration;
        private readonly ILogger<JwtService> logger = logger;

        public TokenResponse? GenerateAccessToken(List<Claim> authClaims)
        {
            try
            {
                var existTokenId = authClaims.Exists(auth => auth.Type == JwtRegisteredClaimNames.Jti);
                if (existTokenId)
                {
                    authClaims.Remove(authClaims.First(auth => auth.Type == JwtRegisteredClaimNames.Jti));
                }

                var tokenId = Guid.NewGuid().ToString();

                _ = int.TryParse(configuration[Config.Config.AccessTokenExpireMinutes], out int AccessTokenExpireMinutes);
                var expiration = DateTime.UtcNow.AddMinutes(AccessTokenExpireMinutes);

                var secretKeyHash = Encoding.UTF8.GetBytes(configuration[Config.Config.AccessTokenSecret] ?? throw new InvalidOperationException("Secret access token not found"));

                var authSecret = new SymmetricSecurityKey(secretKeyHash);

                authClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, tokenId));

                var jwtToken = new JwtSecurityToken
                (
                    issuer: configuration[Config.Config.Issues],
                    audience: configuration[Config.Config.Audience],
                    expires: expiration,
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

                var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                return new TokenResponse()
                {
                    TokenId = tokenId,
                    Expiration = expiration,
                    Token = token
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred while create token : {ex.Message} : at {DateTime.UtcNow}");
                return default;
            }
        }

        public TokenResponse? GenerateRefreshToken(List<Claim> authClaims)
        {
            try
            {
                var existTokenId = authClaims.Exists(auth => auth.Type == JwtRegisteredClaimNames.Jti);
                if (existTokenId)
                {
                    authClaims.Remove(authClaims.First(auth => auth.Type == JwtRegisteredClaimNames.Jti));
                }

                var refreshTokenId = Guid.NewGuid().ToString();

                _ = int.TryParse(configuration[Config.Config.RefreshTokenExpireMinutes], out int RefreshTokenExpireMinutes);
                var expiration = DateTime.UtcNow.AddMinutes(RefreshTokenExpireMinutes);

                var secretKeyHash = Encoding.UTF8.GetBytes(configuration[Config.Config.RefreshTokenSecret] ?? throw new InvalidOperationException("Secret refresh token not found"));

                var authSecret = new SymmetricSecurityKey(secretKeyHash);

                authClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, refreshTokenId));

                var refreshToken = new JwtSecurityToken
                (
                    issuer: configuration[Config.Config.Issues],
                    audience: configuration[Config.Config.Audience],
                    expires: expiration,
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(refreshToken);
                return new TokenResponse
                {
                    TokenId = refreshTokenId,
                    Token = token,
                    Expiration = expiration
                };

            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred while create refresh token : {ex.Message} at {DateTime.UtcNow}");
                return default;
            }
        }

        public List<Claim> GetClaimsFormToken(string? token)
        {
            logger.LogError("cccc= Lỗi đây này");
            var tokenHandler = new JwtSecurityTokenHandler();

            if (tokenHandler.ReadToken(token) is JwtSecurityToken securityToken)
            {
                logger.LogError("111m= Lỗi đây này");
                return securityToken.Claims.ToList();
            }
            logger.LogError("Lỗi đây này");
            return [];

        }
        public bool CheckValidateToken(string? token, dynamic op)
        {
            var issues = configuration[Config.Config.Issues];
            var audience = configuration[Config.Config.Audience];
            var scret = configuration[op];

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(scret ?? "")),
                ValidateIssuer = true,
                ValidIssuer = issues,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return validatedToken != null;
        }

    }
}
