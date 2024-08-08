using System.Security.Claims;

namespace EGeek.Common.Help;

public static class ClaimsExtension
{
    public static string GetEmail(this ClaimsPrincipal principal)
    {
        var emailClaim = principal.FindFirst(ClaimTypes.Email);
        if(emailClaim == null || string.IsNullOrWhiteSpace(emailClaim.Value))
            throw new ApplicationException("Email claim not found");

        return emailClaim.Value;
    }
}