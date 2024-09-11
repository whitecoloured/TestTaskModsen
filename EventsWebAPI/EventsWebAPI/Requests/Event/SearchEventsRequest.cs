using EventsWebAPI.Enums;
using System;

namespace EventsWebAPI.Requests.Event
{
    public record SearchEventsRequest(string SearchValue, EventCategory? SearchCategory, DateTime? SearchDate);
}
