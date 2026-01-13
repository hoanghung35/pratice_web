using Content_App.Domain.Enums;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Content_App.App.Policies
{
    public class RolePolicy
    {
        static bool HasRole(AuthorizationHandlerContext ctx, RoleCode required)
        {
            var roleClaim = ctx.User.FindFirst(JwtClaimConstants.Role)?.Value;
            if (roleClaim == null) return false;

            var userRole = Enum.Parse<RoleCode>(roleClaim);
            return userRole >= required;
        }
    }
}
