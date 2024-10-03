using System;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.GetAllEvents.Responses
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
}
