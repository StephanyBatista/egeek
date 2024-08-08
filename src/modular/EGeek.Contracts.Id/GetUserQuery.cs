using MediatR;

namespace EGeek.Contracts.Id;

public record GetUserQuery(string Email) : IRequest<GetUserResponse>;
