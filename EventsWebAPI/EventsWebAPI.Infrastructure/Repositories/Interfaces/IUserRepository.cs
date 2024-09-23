using EventsWebAPI.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task <User> GetUserByIdAsync(Guid ID, CancellationToken ct);
        Task <User> GetUserByEmailAsync(string Email, CancellationToken ct);
        Task CreateUserAsync(User user, CancellationToken ct);

        Task DeleteUserAsync(User user, CancellationToken ct);

    }
}
