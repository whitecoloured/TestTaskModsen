using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventsWebAPI.Core.Models;
using EventsWebAPI.Core.Enums;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using EventsWebAPI.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventsWebAPI.Infrastructure.Repositories.Implementations
{
    public class EventRepository : IEventRepository
    {

        private readonly EventDbContext _context;

        public EventRepository(EventDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Event>> GetAllEventsAsQueryableAsync()
        {
            var data = await Task.Run(()=> _context.Events.AsQueryable());

            return data;
        }

        public IQueryable<Event> GetEventsBySearch(IQueryable<Event> events, string SearchValue)
        {
            return events.Where(s => s.Name.ToLower().Contains(SearchValue.ToLower()) ||
                    s.EventPlace.ToLower().Contains(SearchValue.ToLower()));
        }

        public IQueryable<Event> GetEventsByCategory(IQueryable<Event> events, EventCategory? category)
        {
            return events.Where(s => s.Category == category);
        }

        public IQueryable<Event> GetEventsByDate(IQueryable<Event> events, DateTime? date)
        {
            return events.Where(s => s.EventDate.Date == date.Value.Date);
        }

        public IQueryable<Event> GetSortedEvents(IQueryable<Event> events, string SortItem, bool OrderByAsc)
        {
            Expression<Func<Event, object>> orderKey = SortItem == "category" ? e => e.Category : e => e.EventPlace;
            if (OrderByAsc)
            {
                return events.OrderBy(orderKey);
            }
            else return events.OrderByDescending(orderKey);
        }

        public IQueryable<Event> GetEventsByPage(IQueryable<Event> events,int page)
        {
            return events.Skip((page - 1) * 3).Take(3);
        }

        public async Task<Event> GetEventByIdAsync(Guid ID, CancellationToken ct)
        {
            return await _context.Events.FindAsync(new object[] { ID}, ct);
        }

        public async Task<Event> GetEventByIdIncludingMembersAsync(Guid ID, CancellationToken ct)
        {
            return await _context.Events.Include(e => e.Members)
                            .FirstOrDefaultAsync(e => e.Id == ID,ct);
        }

        public async Task CreateEventAsync(Event _event, CancellationToken ct)
        {
            await _context.Events.AddAsync(_event,ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateEventAsync(Event _event, CancellationToken ct)
        {

            _context.Events.Update(_event);
            await _context.SaveChangesAsync(ct);
        }
        public async Task DeleteEventAsync(Event _event, CancellationToken ct)
        {
            _context.Events.Remove(_event);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> HasAnySameEventsAsync(Event request, CancellationToken ct)
        {
            return await _context.Events.AnyAsync(e=> e.Name.Trim().ToLower()==request.Name.Trim().ToLower()
                            && e.Description.Trim().ToLower()==request.Description.Trim().ToLower()
                            && e.EventDate==request.EventDate
                            && e.EventPlace.Trim().ToLower()==request.EventPlace.Trim().ToLower()
                            && e.Category==request.Category
                            && e.MaxAmountOfMembers==request.MaxAmountOfMembers
                            && e.ImageURL==request.ImageURL,
                            ct);
        }
    }
}
