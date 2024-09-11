using EventsWebAPI.Models;
using EventsWebAPI.Requests.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsWebAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task <User> GetUserByIdAsync(Guid ID);
        Task <User> GetUserByEmailAsync(string Email);
        Task CreateUserAsync(RegisterUserRequest request);

        Task DeleteUserAsync(User user);

    }
}
