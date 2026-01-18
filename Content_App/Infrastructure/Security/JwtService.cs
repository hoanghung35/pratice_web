using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Content_App.App.Interfaces;
using Content_App.Domain.Entities;
using Content_App.Shared.Constants;
using Microsoft.IdentityModel.Tokens;

namespace Content_App.Infrastructure.Security
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Account account)
        {
            var claims = BuildClaims(account);

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static IEnumerable<Claim> BuildClaims(Account account)
        {
            return new List<Claim>
        {
            new(JwtClaimConstants.UserId, account.Id.ToString()),
            new(JwtClaimConstants.UserCode, account.UserCode),
            new(JwtClaimConstants.Role, account.Role.RoleNum),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };
        }
    }
}
