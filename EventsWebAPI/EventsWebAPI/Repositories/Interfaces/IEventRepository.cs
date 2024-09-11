using EventsWebAPI.Models;
using EventsWebAPI.Requests.Event;
using EventsWebAPI.Requests.Event.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsWebAPI.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<IQueryable<Event>> GetAllEventsAsQueryableAsync(SearchEventsRequest searchRequest, FilterEventsRequest fliterRequest);

        IQueryable<Event> GetEventsByPage(IQueryable<Event>events,int page);
        Task<Event> GetEventByIdAsync(Guid ID);

        Task<Event> GetEventByIdIncludingMembersAsync(Guid ID);

        Task CreateEventAsync(CreateEventRequest request);

        Task UpdateEventAsync(Event _event,UpdateEventRequest request);

        Task DeleteEventAsync(Event _event);

        Task<bool> HasAnySameEventsAsync(EventRequest request);

    }
}
