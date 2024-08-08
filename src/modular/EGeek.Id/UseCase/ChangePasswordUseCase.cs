using EGeek.Id.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Id.UseCase;

internal static class ChangePasswordUseCase
{
    [Authorize]
    public static async Task<Ok>  Action(
        ChangePasswordRequest request, IUserRepository userRepository)
    {
        BadException.ThrowIfNullOrEmpt(request.Email, nameof(request.Email));
        BadException.ThrowIfNullOrEmpt(request.CurrentPassword, nameof(request.CurrentPassword));
        BadException.ThrowIfNullOrEmpt(request.NewPassword, nameof(request.NewPassword));

        var user = await userRepository.GetByEmailAndPassword(request.Email, request.CurrentPassword);
        BadException.ThrowIfWithMessage(user == null, "Email or password is incorrect");

        await userRepository.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);
        
        return TypedResults.Ok();
    }
}