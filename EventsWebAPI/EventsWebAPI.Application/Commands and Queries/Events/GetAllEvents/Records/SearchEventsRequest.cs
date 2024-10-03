using EventsWebAPI.Core.Enums;
using System;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.GetAllEvents.Records
{
    public record SearchEventsRequest(string SearchValue, EventCategory? SearchCategory, DateTime? SearchDate);
}
