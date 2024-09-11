using EventsWebAPI.Enums;
using EventsWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsWebAPI.Dto_s.Events
{
    public class ReadEventDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime EventDate { get; set; }
        public string EventPlace { get; set; }
        public string Category { get; set; }
        public string ImageURL { get; set; }
    }
    public static class ReadEventsDTO
    {
        public static ReadEventDto ToEventDto(this Event _event)
        {
            return new() { Id = _event.Id, Name = _event.Name,
                EventDate = _event.EventDate,
                EventPlace = _event.EventPlace,
                Category = Enum.GetName(typeof(EventCategory), _event.Category),
                ImageURL = _event.ImageURL };
        }
    }
}
