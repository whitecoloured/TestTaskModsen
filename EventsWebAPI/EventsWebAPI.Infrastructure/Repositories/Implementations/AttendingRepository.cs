using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventsWebAPI.Core.Models;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using EventsWebAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace EventsWebAPI.Infrastructure.Repositories.Implementations
{
    public class AttendingRepository : IAttendingRepository
    {
        private readonly EventDbContext _context;

        public AttendingRepository(EventDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<AttendingInfo>> GetMembersEventsAsync(Guid UserID)
        {
            //e.toMembersEventDTO()
            return await Task.Run(()=>_context.Attendings
                .Where(u => u.UserID == UserID)
                .Include(e => e.Event)
                .AsQueryable());
        }

        public async Task<IQueryable<AttendingInfo>> GetMembersByEventAsync(Guid EventID)
        {
            //u.User.toMemberDto()
            return await Task.Run(()=>_context.Attendings
                .Where(e => e.EventID == EventID)
                .Include(u => u.User)
                .AsQueryable());
        }

        public async Task<AttendingInfo> GetAttendingAsync(Guid EventID, Guid UserID, CancellationToken ct)
        {
            return await _context.Attendings.FirstOrDefaultAsync(a => a.EventID == EventID && a.UserID == UserID, ct);
        }

        public async Task CreateAttendingAsync(Event _event, User user, CancellationToken ct)
        {
            AttendingInfo info = new()
            {
                Event = _event,
                User = user
            };
            await _context.Attendings.AddAsync(info, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAttendingAsync(AttendingInfo info, CancellationToken ct)
        {
            _context.Attendings.Remove(info);
            await _context.SaveChangesAsync(ct);
        }
    }
}
