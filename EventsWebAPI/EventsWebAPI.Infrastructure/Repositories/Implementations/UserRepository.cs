using System;
using System.Threading;
using System.Threading.Tasks;
using EventsWebAPI.Context;
using EventsWebAPI.Core.Models;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebAPI.Infrastructure.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly EventDbContext _context;
        public UserRepository(EventDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(Guid ID, CancellationToken ct)
        {
            return await _context.Users.FirstOrDefaultAsync(u=> u.Id==ID,ct);
        }

        public async Task<User> GetUserByEmailAsync(string Email, CancellationToken ct)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == Email,ct);
        }

        public async Task CreateUserAsync(User user, CancellationToken ct)
        {
            await _context.Users.AddAsync(user,ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteUserAsync(User user, CancellationToken ct)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);
        }
    }
}
