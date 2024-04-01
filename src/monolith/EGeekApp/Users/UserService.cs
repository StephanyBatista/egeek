using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace EGeekapp.Users;

public class UserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserResponse> Create(UserRequest userRequest, string creatorEmail = null)
    {
        if (string.IsNullOrEmpty(userRequest.Name) || string.IsNullOrEmpty(userRequest.Email) || string.IsNullOrEmpty(userRequest.Password))
        {
            throw new ArgumentException("Name, Email and Password are required fields");
        }

        var user = new User
        {
            UserName = userRequest.Name,
            Email = userRequest.Email
        };
        
        if(!string.IsNullOrEmpty(creatorEmail))
        {
            var creator = await _userManager.FindByEmailAsync(creatorEmail);
            if (creator == null)
            {
                throw new ArgumentException("Creator email is not valid");
            }

            var creatorClaims = await _userManager.GetClaimsAsync(creator);
            if (!creatorClaims.Any(c => c.Type == "Worker" && c.Value == "True"))
            {
                throw new ArgumentException("Creator does not have the Worker claim");
            }
        }

        var result = await _userManager.CreateAsync(user, userRequest.Password);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(creatorEmail)){
                var claim = new Claim("Worker", "True");
                await _userManager.AddClaimAsync(user, claim);
            }

            return new UserResponse
            {
                Name = userRequest.Name,
                Email = userRequest.Email
            };
        }

        throw new Exception(result.Errors.First().Description);
    }
    
    public async Task<UserResponse> GetBy(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email
            };
        }

        return null;
    }
    
    public async Task<bool> ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        if (string.IsNullOrEmpty(changePasswordRequest.Email) || string.IsNullOrEmpty(changePasswordRequest.CurrentPassword) || string.IsNullOrEmpty(changePasswordRequest.NewPassword))
        {
            throw new ArgumentException("Email, Current Password and New Password are required fields");
        }
        
        var user = await _userManager.FindByEmailAsync(changePasswordRequest.Email);

        if (user != null)
        {
            var result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword);

            if (result.Succeeded)
            {
                return true;
            }
        }

        return false;
    }
    
    public async Task<bool> Authenticate(AuthenticateRequest authenticateRequest)
    {
        var user = await _userManager.FindByEmailAsync(authenticateRequest.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, authenticateRequest.Password))
        {
            return true;
        }

        return false;
    }
}