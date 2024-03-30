using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EGeekapp.Users;

public class UserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserResponse> Create(UserRequest userRequest)
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

        var result = await _userManager.CreateAsync(user, userRequest.Password);

        if (result.Succeeded)
        {
            return new UserResponse
            {
                Name = userRequest.Name,
                Email = userRequest.Email
            };
        }

        throw new Exception(result.Errors.First().Description);
    }

    public async Task<UserResponse> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

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

    public async Task<List<UserResponse>> GetAll()
    {
        var users = await _userManager.Users.ToListAsync();

        return users.Select(user => new UserResponse
        {
            Id = user.Id,
            Name = user.UserName,
            Email = user.Email
        }).ToList();
    }

    public async Task<UserResponse> Update(UserRequest userRequest)
    {
        if (string.IsNullOrEmpty(userRequest.Name) || string.IsNullOrEmpty(userRequest.Email))
        {
            throw new ArgumentException("Name and Email are required fields");
        }
        var user = await _userManager.FindByEmailAsync(userRequest.Email);

        if (user != null)
        {
            user.UserName = userRequest.Email;
            user.Email = userRequest.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new UserResponse
                {
                    Name = userRequest.Name,
                    Email = userRequest.Email
                };
            }
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