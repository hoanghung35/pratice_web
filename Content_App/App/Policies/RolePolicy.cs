using Content_App.Domain.Enums;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Content_App.App.Policies
{
    public class RolePolicy
    {
        public bool HasRole(AuthorizationHandlerContext atx, RoleKey rk)
        {
            var roleClaim = atx.User.FindFirst(JwtClaimConstant.Role)?.Value;
            if (roleClaim == null) return false;

            var userRole = Enum.Parse<RoleKey>(roleClaim);
            return userRole >= rk;
        }
    }
}
