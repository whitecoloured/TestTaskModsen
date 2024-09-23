using System;

namespace EventsWebAPI.Application.Dto_s.Responses.Events
{
    public class MemberEventDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
