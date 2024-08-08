using EGeek.Id.UseCase;
using Microsoft.AspNetCore.Identity;

namespace EGeek.Id.Domain;

internal class User : IdentityUser
{
    public string Name { get; private set; }

    internal User() { }
    
    internal User(CreateUserRequest request)
    {
        BadException.ThrowIfNullOrEmpt(request.Email, nameof(Email));
        BadException.ThrowIfEmailInvalid(request.Email, nameof(Email));
        BadException.ThrowIfNullOrEmpt(request.Name, nameof(Name));

        Email = request.Email;
        UserName = Email;
        Name = request.Name;
    }
}