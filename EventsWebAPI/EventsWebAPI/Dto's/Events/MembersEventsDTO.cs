using EventsWebAPI.Enums;
using EventsWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsWebAPI.Dto_s.Events
{
    public class MemberEventDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
    public static class MembersEventsDTO
    {
        public static MemberEventDTO toMembersEventDTO(this AttendingInfo info)
        {
            return new() { Id=info.Event.Id, Name=info.Event.Name, RegistrationDate=info.RegistrationDate  };
        }
    }
}
