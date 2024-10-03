using MediatR;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace EventsWebAPI.Application.Commands_and_Queries.Attendings.CancelEvent
{
    public record CancelEventCommand(Guid EventID, KeyValuePair<string, StringValues> HeaderData) : IRequest;
}
