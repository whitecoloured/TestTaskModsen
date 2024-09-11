using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsWebAPI.Models;
using EventsWebAPI.Dto_s.Members;
using EventsWebAPI.Dto_s.Events;
using EventsWebAPI.Repositories.Interfaces;
using EventsWebAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace EventsWebAPI.Repositories.Implementations
{
    public class AttendingRepository : IAttendingRepository
    {
        private readonly EventDbContext _context;

        public AttendingRepository(EventDbContext context)
        {
            _context = context;
        }

        public async Task<List<MemberEventDTO>> GetMembersEventsAsync(Guid UserID)
        {
            return await _context.Attendings
                .Where(u => u.UserID == UserID)
                .Include(e => e.Event)
                .Select(e => e.toMembersEventDTO())
                .ToListAsync();
        }

        public async Task<List<MemberDto>> GetMembersByEventAsync(Guid EventID)
        {
            return await _context.Attendings
                .Where(e => e.EventID == EventID)
                .Include(u => u.User)
                .Select(u => u.User.toMemberDto())
                .ToListAsync();
        }

        public async Task<AttendingInfo> GetAttendingAsync(Guid EventID, Guid UserID)
        {
            return await _context.Attendings.FirstOrDefaultAsync(a => a.EventID == EventID && a.UserID == UserID);
        }

        public async Task CreateAttendingAsync(Event _event, User user)
        {
            AttendingInfo info = new()
            {
                Event = _event,
                User = user
            };
            await _context.Attendings.AddAsync(info);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAttendingAsync(AttendingInfo info)
        {
            _context.Attendings.Remove(info);
            await _context.SaveChangesAsync();
        }
    }
}
