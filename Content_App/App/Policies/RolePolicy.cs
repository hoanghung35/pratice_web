using Content_App.Domain.Enums;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Content_App.App.Policies
{
    public class RolePolicy
    {
        static bool HasRole(AuthorizationHandlerContext ctx, RoleKey required)
        {
            var roleClaim = ctx.User.FindFirst(JwtClaimConstants.Role)?.Value;
            if (roleClaim == null) return false;

            var userRole = Enum.Parse<RoleKey>(roleClaim);
            return userRole >= required;
        }
    }
}
