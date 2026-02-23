using Content_App.App.Interfaces;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Content_App.Shared.Constants;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Content_App.Infrastructure.Security
{
    public class JwtService:IAuthService
    {
        private readonly IConfiguration _config;
        private readonly LogDbContext _context;

        public JwtService(IConfiguration config, LogDbContext context)
        {
            this._config = config;
            this._context = context;
        }

        public string GenerateAccessToken(Account user)
        {
            var emp = _context.Employees.FirstOrDefault(e => e.EmpCode == user.UserCode);
            var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            var claims = BuildClaims(user, emp!, role!);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!.ToString()));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"]!.ToString(),
                audience: _config["Jwt:Audience"]!.ToString(),
                claims: claims,
                expires: DateTime.UtcNow.AddHours(7).AddMinutes(int.Parse(_config["Jwt:ExpirateTime"]!.ToString())),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static IEnumerable<Claim> BuildClaims(Account account, Employee emp, Role role)
        {
            return new List<Claim>
            {
                new Claim(JwtClaimConstant.UserName, emp == null ? "Welcomeback":emp.FullName),
                new Claim(JwtClaimConstant.Role, role.Descreption!),
                new Claim(JwtClaimConstant.UserId, account.Id.ToString()),
                new Claim(JwtClaimConstant.RoleId, role.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }
    }
}
