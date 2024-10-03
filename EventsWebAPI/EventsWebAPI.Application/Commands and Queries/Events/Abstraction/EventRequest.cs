using EventsWebAPI.Core.Enums;
using System;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.Abstraction
{
    public abstract class EventRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string EventPlace { get; set; }
        public EventCategory Category { get; set; }
        public int MaxAmountOfMembers { get; set; }
        public string ImageURL { get; set; }
    }
}
