using System.Security.Claims;

namespace EGeekApp.Helper;

public static class UserHelper
{
    public static void ThrowExceptionIfUserIsNotWorker(IEnumerable<Claim> claims)
    {
        if (!claims.Any(c => c is { Type: ClaimTypes.Role, Value: "worker" })) 
            throw new UnauthorizedAccessException("User is not allowed to update order");
    }

    public static string GetEmail(IEnumerable<Claim> claims)
    {
        return claims.First(c => c.Type == ClaimTypes.Email).Value;
    }
}