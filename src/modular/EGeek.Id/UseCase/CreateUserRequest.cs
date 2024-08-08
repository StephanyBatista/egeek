namespace EGeek.Id.UseCase;

internal class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "public";
}