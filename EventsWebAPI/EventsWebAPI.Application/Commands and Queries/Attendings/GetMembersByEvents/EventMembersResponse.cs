using System;

namespace EventsWebAPI.Application.Commands_and_Queries.Attendings.GetMembersByEvents
{
    public class EventMembersResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
