using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsWebAPI.Dto_s.Events
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
