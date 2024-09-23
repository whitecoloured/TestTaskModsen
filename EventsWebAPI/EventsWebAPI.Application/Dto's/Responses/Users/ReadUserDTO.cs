
using System;

namespace EventsWebAPI.Application.Dto_s.Responses.Users
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
