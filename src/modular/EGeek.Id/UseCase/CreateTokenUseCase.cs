using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EGeek.Id.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EGeek.Id.UseCase;

internal static class CreateTokenUseCase
{
    [AllowAnonymous]
    public static async Task<Results<Ok<object>, UnauthorizedHttpResult>> Action(
        CreateTokenRequest request, IUserRepository userRepository, IConfiguration configuration)
    {
        BadException.ThrowIfNullOrEmpt(request.Email, nameof(request.Email));
        BadException.ThrowIfNullOrEmpt(request.Password, nameof(request.Password));

        var user = await userRepository.GetByEmailAndPassword(request.Email, request.Password);
        
        if(user == null)
            return TypedResults.Unauthorized();
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
            configuration["Jwt:Issuer"],
            [new Claim(ClaimTypes.Email, request.Email)],
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        object tokenObj = new { token = new JwtSecurityTokenHandler().WriteToken(token) };
        return TypedResults.Ok(tokenObj);
    }
}