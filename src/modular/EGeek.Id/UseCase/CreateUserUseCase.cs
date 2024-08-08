using EGeek.Id.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EGeek.Id.UseCase;

internal static class CreateUserUseCase
{
    [AllowAnonymous]
    public static async Task<Created<string>> Action(CreateUserRequest request, IUserRepository userRepository)
    {
        BadException.ThrowIfNullOrEmpt(request.Password, nameof(request.Password));
        BadException.ThrowIf(request.Role != "worker" && request.Role != "public", nameof(request.Role));
        
        var user = new User(request);
        var id = await userRepository.CreateAsync(user, request.Password, request.Role);
        
        return TypedResults.Created($"/user/{id}", id);
    }
}