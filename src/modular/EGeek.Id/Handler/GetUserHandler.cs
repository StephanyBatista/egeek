using System.Security.Claims;
using EGeek.Contracts.Id;
using EGeek.Id.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EGeek.Id.Handler;

internal class GetUserHandler(IUserRepository userRepository, UserManager<User> userManager)
    : IRequestHandler<GetUserQuery, GetUserResponse?>
{
    public async Task<GetUserResponse?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        BadException.ThrowIfNullOrEmpt(request.Email, nameof(request.Email));
        var user = await userRepository.GetByEmail(request.Email);
        if (user == null)
            return null;
        var claims = await userManager.GetClaimsAsync(user);
        var workerClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        var isWorker = false;
        if(workerClaim != null)
            isWorker = workerClaim.Value == "worker";
            
        return new GetUserResponse(user.Email!, isWorker, user.Name);
    }
}