using System;
using MediatR;

namespace EventsWebAPI.Application.Commands_and_Queries.Users.Register
{
    public record RegisterUserCommand(string Email, string Name, string Surname, DateTime? BirthDate) : IRequest<string>;
}
