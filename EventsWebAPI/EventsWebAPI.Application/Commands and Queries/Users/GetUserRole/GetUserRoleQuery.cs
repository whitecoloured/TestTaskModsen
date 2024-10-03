using MediatR;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace EventsWebAPI.Application.Commands_and_Queries.Users.GetUserRole
{
    public record GetUserRoleQuery(KeyValuePair<string, StringValues> HeaderData) : IRequest<string>;
}
