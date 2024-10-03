
using System;

namespace EventsWebAPI.Application.Commands_and_Queries.Users.GetProfileInfo
{
    public class GetProfileInfoResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
