using EventsWebAPI.Application.Commands_and_Queries.Events.GetAllEvents.Records;
using EventsWebAPI.Application.Commands_and_Queries.Events.GetAllEvents.Responses;
using MediatR;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.GetAllEvents
{
    public record GetAllEventsQuery(SearchEventsRequest SearchRequest, FilterEventsRequest FilterRequest, int Page=-1) : IRequest<DisplayEventsDTO>;
}
