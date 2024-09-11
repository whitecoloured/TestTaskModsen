using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsWebAPI.Context;
using EventsWebAPI.Models;
using EventsWebAPI.Repositories.Interfaces;
using EventsWebAPI.Requests.User;
using Microsoft.EntityFrameworkCore;

namespace EventsWebAPI.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly EventDbContext _context;
        public UserRepository(EventDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(Guid ID)
        {
            return await _context.Users.FindAsync(ID);
        }

        public async Task<User> GetUserByEmailAsync(string Email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
        }

        public async Task CreateUserAsync(RegisterUserRequest request)
        {
            User user = new()
            {
                Name = request.Name,
                Surname = request.Surname,
                BirthDate = request.BirthDate,
                Email = request.Email
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
