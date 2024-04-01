using System.Security.Claims;
using EGeekapp.Users;
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
    public async Task<IActionResult> Create([FromBody] UserRequest userRequest)
    {
        var userResponse = await _userService.Create(userRequest);
        return Ok(userResponse);
    }
    
    [HttpPost("worker")] 
    public async Task<IActionResult> CreateWorker([FromBody] UserRequest userRequest)
    {
        var userResponse = await _userService.Create(userRequest, User.Identity.Name);
        return Ok(userResponse);
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userResponse = await _userService.GetBy(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        
        return Ok(userResponse);
    }
    
    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
    {
        await _userService.ChangePassword(changePasswordRequest);
        return Ok();
    }
}