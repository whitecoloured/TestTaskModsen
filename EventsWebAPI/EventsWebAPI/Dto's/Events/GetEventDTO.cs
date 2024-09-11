using EventsWebAPI.Enums;
using EventsWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsWebAPI.Dto_s.Events
{
    public class GetEventDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string EventPlace { get; set; }
        public EventCategory Category { get; set; }
        public int MaxAmountOfMembers { get; set; }
        public string ImageURL { get; set; }
    }
    public static class GetEventsDTO
    {
        public static GetEventDTO ToGetEventDto(this Event _event)
        {
            return new()
            {
                Id = _event.Id,
                Name = _event.Name,
                Description = _event.Description,
                EventDate = _event.EventDate,
                EventPlace = _event.EventPlace,
                Category = _event.Category,
                MaxAmountOfMembers = _event.MaxAmountOfMembers,
                ImageURL = _event.ImageURL
            };
        }
    }
}
