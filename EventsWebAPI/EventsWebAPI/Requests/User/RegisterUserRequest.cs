using System;

namespace EventsWebAPI.Requests.User
{
    public record RegisterUserRequest(string Email, string Name, string Surname, DateTime? BirthDate);
}
