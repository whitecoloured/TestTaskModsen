using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsWebAPI.Models;

namespace EventsWebAPI.Dto_s.Members
{
    public class MemberDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
    public static class EventMembersDTO
    {
        public static MemberDto toMemberDto(this User user)
        {
            return new() { Id = user.Id, Name = user.Name, Surname = user.Surname };
        }

    }
}
