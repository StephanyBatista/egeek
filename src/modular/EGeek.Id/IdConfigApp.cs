using EGeek.Id.UseCase;
using Microsoft.AspNetCore.Builder;

namespace EGeek.Id;

public static class IdConfigApp
{
    public static void Apply(WebApplication app)
    {
        app.MapPost("/v1/users", CreateUserUseCase.Action);
        app.MapPost("/v1/users/change-password", ChangePasswordUseCase.Action);
        app.MapGet("/v1/users/me", MeUseCase.Action);
        app.MapPost("/v1/token", CreateTokenUseCase.Action);
    }
}