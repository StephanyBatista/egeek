namespace EGeek.Contracts.Id;

public record GetUserResponse(string Email, bool IsWorker, string Name);
