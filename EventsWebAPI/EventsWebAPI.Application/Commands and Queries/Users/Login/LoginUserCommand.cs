
using MediatR;

namespace EventsWebAPI.Application.Commands_and_Queries.Users.Login
{
    public record LoginUserCommand(string Email) : IRequest<string>;
}
