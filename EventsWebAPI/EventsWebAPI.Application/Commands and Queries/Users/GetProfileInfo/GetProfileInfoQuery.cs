using MediatR;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace EventsWebAPI.Application.Commands_and_Queries.Users.GetProfileInfo
{
    public record GetProfileInfoQuery(KeyValuePair<string, StringValues> HeaderData):IRequest<GetProfileInfoResponse>;
}
