using System.Security.Claims;
using EGeekApp.Request;
using EGeekApp.Response;
using EGeekDomain;
using Microsoft.AspNetCore.Identity;

namespace EGeekApp.Service;

public class UserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Create(UserRequest request)
    {
        if (string.IsNullOrEmpty(request.Name)) throw new ArgumentException("Name is required"); 
        if (string.IsNullOrEmpty(request.Email)) throw new ArgumentException("Email is required"); 
        if (string.IsNullOrEmpty(request.Password)) throw new ArgumentException("Password is required");

        var user = new User
        {
            Email = request.Email,
            UserName = request.Email
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
        
        var roleClaim = new Claim(ClaimTypes.Role, request.Role == "worker" ? "worker": "public");
        var emailClaim = new Claim(ClaimTypes.Email, request.Email);
        var nameClaim = new Claim(ClaimTypes.Name, request.Name);
        
        await _userManager.AddClaimsAsync(user, [roleClaim, emailClaim, nameClaim]);

        return user.Id;
    }
    
    public async Task<UserResponse> GetBy(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if(user == null) throw new ArgumentException("User not found");
        var claims = await _userManager.GetClaimsAsync(user);

        return new UserResponse
        {
            Id = user.Id,
            Name = claims.First(c => c.Type == ClaimTypes.Name).Value,
            Email = user.Email
        };
    }
    
    public async Task<bool> ChangePassword(ChangePasswordRequest request)
    {
        if (string.IsNullOrEmpty(request.Email)) throw new ArgumentException("Email is required"); 
        if (string.IsNullOrEmpty(request.CurrentPassword)) throw new ArgumentException("CurrentPassword is required");
        if (string.IsNullOrEmpty(request.NewPassword)) throw new ArgumentException("NewPassword is required");
        
        var user = await _userManager.FindByEmailAsync(request.Email);
        if(user == null) throw new ArgumentException("User not found");

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        return result.Succeeded;
    }
    
    public async Task<(User, IList<Claim>)> Authenticate(AuthenticateRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
        {
            var claims = await _userManager.GetClaimsAsync(user);
            return (user, claims);
        }

        return (null, null);
    }
}