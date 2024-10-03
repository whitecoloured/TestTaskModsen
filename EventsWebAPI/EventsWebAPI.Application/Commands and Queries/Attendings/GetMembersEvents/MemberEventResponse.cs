using System;

namespace EventsWebAPI.Application.Commands_and_Queries.Attendings.GetMembersEvents
{
    public class MemberEventResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
