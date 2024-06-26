using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EGeekApp.Request;
using EGeekApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace EGeekapi.Controllers; 

[Route("v1/token")]
public class TokenController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IConfiguration _configuration;

    public TokenController(UserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> GenerateToken([FromBody] AuthenticateRequest request)
    {
        var (user, claimsFromUser) = await _userService.Authenticate(request);
        
        if(user == null)
            return Unauthorized();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claimsFromUser,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

}