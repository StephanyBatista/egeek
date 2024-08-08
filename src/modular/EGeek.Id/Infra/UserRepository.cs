using System.Security.Claims;
using EGeek.Id.Domain;
using Microsoft.AspNetCore.Identity;

namespace EGeek.Id.Infra;

internal class UserRepository(UserManager<User> userManager) : IUserRepository

{
    public async Task<string> CreateAsync(User user, string password, string role)
    {
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new ApplicationException("Error creating user");
        
        var roleClaim = new Claim(ClaimTypes.Role, role);
        var emailClaim = new Claim(ClaimTypes.Email, user.Email);
        
        await userManager.AddClaimsAsync(user, [roleClaim, emailClaim]);

        return user.Id;
    }
    
    public async Task<User?> GetByEmail(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return null;
        
        var result = await userManager.CheckPasswordAsync(user, password);
        if(!result)
            return null;

        return user;
    }

    public async Task ChangePasswordAsync(User user, string currentPassword, string newPassword)
    {
        var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        BadException.ThrowIfWithMessage(result.Errors?.Any() == true, string.Join(string.Empty, result.Errors!.Select(e => e.Description)));
    }
}