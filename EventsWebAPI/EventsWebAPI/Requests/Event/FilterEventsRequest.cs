using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsWebAPI.Requests.Event
{
    public record FilterEventsRequest(string SortItem, bool OrderByAsc);
}
