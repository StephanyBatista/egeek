using System.Security.Claims;
using EGeek.Id.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Id.UseCase;

internal static class MeUseCase
{
    [Authorize]
    public static async Task<Ok<MeResponse>>  Action(ClaimsPrincipal principal, IUserRepository userRepository)
    {
        var user = await userRepository.GetByEmail(principal.Claims.First(c => c.Type == ClaimTypes.Email).Value);
        
        return TypedResults.Ok(new MeResponse(
            user!.Id,
            user.Email!,
            user.Name
        ));
    }
}