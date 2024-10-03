using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using MediatR;

namespace EventsWebAPI.Application.Commands_and_Queries.Attendings.SubscribeToEvent
{
    public record SubToEventCommand(Guid EventID, KeyValuePair<string, StringValues> HeaderData) :IRequest;
}
