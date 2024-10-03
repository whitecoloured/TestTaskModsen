using MediatR;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace EventsWebAPI.Application.Commands_and_Queries.Users.DeleteUserProfile
{
    public record DeleteUserCommand(KeyValuePair<string, StringValues> HeaderData) : IRequest;
}
