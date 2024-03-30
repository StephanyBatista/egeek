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
    
    [HttpGet("me")]
    public async Task<IActionResult> GetById()
    {
        //get the user through the token and set the user response
        var userResponse = new UserResponse
        {
            Email = User.FindFirstValue(ClaimTypes.Email),
            Name = User.FindFirstValue(ClaimTypes.Name),
        };

        return Ok(userResponse);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(UserRequest userRequest)
    {
        try
        {
            var userResponse = await _userService.Update(userRequest);
            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var userResponse = await _userService.GetById(id);
    
        if (userResponse != null)
        {
            return Ok(userResponse);
        }
    
        return NotFound();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }
    
    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
    {
        await _userService.ChangePassword(changePasswordRequest);
        return Ok();
    }
}