using System.Collections.Generic;

namespace EventsWebAPI.Application.Dto_s.Responses.Events
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
