using MediatR;
using EventsWebAPI.Application.Commands_and_Queries.Events.Abstraction;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.CreateEvent
{
    public class CreateEventCommand : EventRequest, IRequest
    {

    }
}
