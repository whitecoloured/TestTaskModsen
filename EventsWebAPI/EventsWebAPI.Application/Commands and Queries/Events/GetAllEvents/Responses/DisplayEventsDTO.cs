using System.Collections.Generic;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.GetAllEvents.Responses
{
    public class DisplayEventsDTO
    {
        public List<ReadEventDto> EventsList { get; set; }
        public int EventsAmount { get; set; }
        public DisplayEventsDTO()
        {

        }
    }
}
