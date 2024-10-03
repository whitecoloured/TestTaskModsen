

namespace EventsWebAPI.Application.Commands_and_Queries.Events.GetAllEvents.Records
{
    public record FilterEventsRequest(string SortItem, bool OrderByAsc);
}
