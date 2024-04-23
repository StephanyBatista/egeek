using EGeekApp.Helper;
using EGeekApp.Request;
using EGeekApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGeekapi.Controllers; 

[Authorize]
[Route("v1/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost] 
    public async Task<IActionResult> Create([FromBody] UserRequest request)
    {
        var userResponse = await _userService.Create(request);
        return Ok(userResponse);
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userResponse = await _userService.GetBy(UserHelper.GetEmail(User.Claims));
        
        return Ok(userResponse);
    }
    
    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        await _userService.ChangePassword(request);
        return Ok();
    }
}