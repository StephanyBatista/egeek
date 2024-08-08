namespace EGeek.Id.Domain;

interface IUserRepository
{
    public Task<string> CreateAsync(User user, string password, string role);
    Task<User?> GetByEmail(string email);
    Task<User?> GetByEmailAndPassword(string email, string password);
    Task ChangePasswordAsync(User user, string currentPassword, string newPassword);
}