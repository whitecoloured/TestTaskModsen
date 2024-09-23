using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventsWebAPI.Core.Models;
using EventsWebAPI.Core.Enums;

namespace EventsWebAPI.Infrastructure.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<IQueryable<Event>> GetAllEventsAsQueryableAsync();
        IQueryable<Event> GetEventsBySearch(IQueryable<Event> events,string SearchValue);
        IQueryable<Event> GetEventsByCategory(IQueryable<Event> events, EventCategory? category);
        IQueryable<Event> GetEventsByDate(IQueryable<Event> events, DateTime? date);
        IQueryable<Event> GetSortedEvents(IQueryable<Event> events, string SortItem, bool OrderByAsc);

        IQueryable<Event> GetEventsByPage(IQueryable<Event>events,int page);
        Task<Event> GetEventByIdAsync(Guid ID, CancellationToken ct);

        Task<Event> GetEventByIdIncludingMembersAsync(Guid ID, CancellationToken ct);

        Task CreateEventAsync(Event _event, CancellationToken ct);

        Task UpdateEventAsync(Event _event, CancellationToken ct);

        Task DeleteEventAsync(Event _event, CancellationToken ct);

        Task<bool> HasAnySameEventsAsync(Event request, CancellationToken ct);

    }
}
