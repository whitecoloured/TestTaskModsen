using EventsWebAPI.Core.Enums;
using System;

namespace EventsWebAPI.Application.Dto_s.Requests.Event
{
    public record SearchEventsRequest(string SearchValue, EventCategory? SearchCategory, DateTime? SearchDate);
}
