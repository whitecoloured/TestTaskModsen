using MediatR;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace EventsWebAPI.Application.Commands_and_Queries.Attendings.GetMembersEvents
{
    public record GetMembersEventsQuery(KeyValuePair<string, StringValues> HeaderData) : IRequest<List<MemberEventResponse>>;
}
