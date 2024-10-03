using System;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.GetEventByIdWithMembers
{
    public class EventInfoResponse
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
}
