using System;

namespace EventsWebAPI.Application.Dto_s.Requests.User
{
    public record RegisterUserRequest(string Email, string Name, string Surname, DateTime? BirthDate);
}
