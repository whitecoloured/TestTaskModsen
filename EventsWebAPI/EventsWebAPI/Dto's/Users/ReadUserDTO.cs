using EventsWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsWebAPI.Dto_s.Users
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
    }
    public static class ReadUserDTO
    {
        public static UserDTO ToUserDto(this User user)
        {
            return new() { Id = user.Id, Name = user.Name, Surname = user.Surname, BirthDate = user.BirthDate };
        }
    }
}
