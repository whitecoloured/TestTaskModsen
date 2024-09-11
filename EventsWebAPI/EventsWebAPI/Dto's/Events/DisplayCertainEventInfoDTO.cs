using EventsWebAPI.Enums;
using EventsWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsWebAPI.Dto_s.Events
{
    public class EventInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string EventPlace { get; set; }
        public string Category { get; set; }
        public int MaxAmountOfMembers { get; set; }
        public string ImageURL { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsExpired { get; set; }
    }
    public static class DisplayCertainEventInfoDTO
    {
        public static EventInfoDto ToEventInfoDto(this Event _event)
        {
            return new()
            {
                Id = _event.Id,
                Name = _event.Name,
                Description=_event.Description,
                EventDate = _event.EventDate,
                EventPlace = _event.EventPlace,
                Category = Enum.GetName(typeof(EventCategory), _event.Category),
                MaxAmountOfMembers=_event.MaxAmountOfMembers,
                IsAvailable=_event.Members.Count<_event.MaxAmountOfMembers,
                IsExpired=_event.EventDate<DateTime.Now,
                ImageURL = _event.ImageURL
            };
        }
    }
}
