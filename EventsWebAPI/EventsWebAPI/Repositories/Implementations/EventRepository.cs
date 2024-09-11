using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsWebAPI.Models;
using EventsWebAPI.Repositories.Interfaces;
using EventsWebAPI.Requests.Event;
using EventsWebAPI.Requests.Event.Abstraction;
using EventsWebAPI.Context;
using Microsoft.EntityFrameworkCore;
using EventsWebAPI.Enums;
using System.Linq.Expressions;

namespace EventsWebAPI.Repositories.Implementations
{
    public class EventRepository : IEventRepository
    {

        private readonly EventDbContext _context;

        public EventRepository(EventDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Event>> GetAllEventsAsQueryableAsync(SearchEventsRequest searchRequest, FilterEventsRequest filterRequest)
        {
            var data = await Task.Run(()=> _context.Events.AsQueryable());

            if (!string.IsNullOrWhiteSpace(searchRequest.SearchValue))
            {
                data = GetEventsBySearchValue(data, searchRequest.SearchValue);
            }
            if (searchRequest.SearchCategory is not null || searchRequest.SearchCategory.ToString() != "")
            {
                data = GetEventsByCategory(data, searchRequest.SearchCategory);
            }
            if (searchRequest.SearchDate is not null)
            {
                data = GetEventsByDate(data, searchRequest.SearchDate);
            }
            if (!string.IsNullOrWhiteSpace(filterRequest.SortItem))
            {
                data = GetSortedEvents(data, filterRequest);
            }

            return data;
        }

        public IQueryable<Event> GetEventsByPage(IQueryable<Event> events,int page)
        {
            return events.Skip((page - 1) * 3).Take(3);
        }

        public async Task<Event> GetEventByIdAsync(Guid ID)
        {
            return await _context.Events.FindAsync(ID);
        }

        public async Task<Event> GetEventByIdIncludingMembersAsync(Guid ID)
        {
            return await _context.Events.Include(e => e.Members)
                            .FirstOrDefaultAsync(e => e.Id == ID);
        }

        public async Task CreateEventAsync(CreateEventRequest request)
        {
            Event _event = new()
            {
                Name = request.Name,
                Description = request.Description,
                EventDate = request.EventDate,
                EventPlace = request.EventPlace,
                Category = request.Category,
                MaxAmountOfMembers = request.MaxAmountOfMembers,
                ImageURL = request.ImageURL
            };
            await _context.Events.AddAsync(_event);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event _event,UpdateEventRequest request)
        {
            _event.Name = request.Name;
            _event.Description = request.Description;
            _event.EventDate = request.EventDate;
            _event.EventPlace = request.EventPlace;
            _event.Category = request.Category;
            _event.MaxAmountOfMembers = request.MaxAmountOfMembers;
            _event.ImageURL = request.ImageURL;

            _context.Events.Update(_event);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteEventAsync(Event _event)
        {
            _context.Events.Remove(_event);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasAnySameEventsAsync(EventRequest request)
        {
            return await _context.Events.AnyAsync(e=> e.Name.Trim().ToLower()==request.Name.Trim().ToLower()
                            && e.Description.Trim().ToLower()==request.Description.Trim().ToLower()
                            && e.EventDate==request.EventDate
                            && e.EventPlace.Trim().ToLower()==request.EventPlace.Trim().ToLower()
                            && e.Category==request.Category
                            && e.MaxAmountOfMembers==request.MaxAmountOfMembers
                            && e.ImageURL==request.ImageURL);
        }

        private static IQueryable<Event> GetEventsBySearchValue(IQueryable<Event> events, string SearchValue)
        {
            return events.Where(s => s.Name.ToLower().Contains(SearchValue.ToLower()) ||
                    s.EventPlace.ToLower().Contains(SearchValue.ToLower()));
        }

        private static IQueryable<Event> GetEventsByCategory(IQueryable<Event> events, EventCategory? category)
        {
            return events.Where(s => s.Category == category);
        }

        private static IQueryable<Event> GetEventsByDate(IQueryable<Event> events, DateTime? date)
        {
            return events.Where(s => s.EventDate.Date == date.Value.Date);
        }

        private static IQueryable<Event> GetSortedEvents(IQueryable<Event> events, FilterEventsRequest request)
        {
            Expression<Func<Event, object>> orderKey=request.SortItem== "category" ? e => e.Category : e => e.EventPlace;
            if (request.OrderByAsc)
            {
                return events.OrderBy(orderKey);
            }
            else return events.OrderByDescending(orderKey);
        }

    }
}
