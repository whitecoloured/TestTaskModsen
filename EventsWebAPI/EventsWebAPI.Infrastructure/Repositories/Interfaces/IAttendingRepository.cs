using EventsWebAPI.Core.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Infrastructure.Repositories.Interfaces
{
    public interface IAttendingRepository
    {
        Task<IQueryable<AttendingInfo>> GetMembersEventsAsync(Guid UserID);
        Task<IQueryable<AttendingInfo>> GetMembersByEventAsync(Guid EventID);
        Task<AttendingInfo> GetAttendingAsync(Guid EventID, Guid UserID, CancellationToken ct);
        Task CreateAttendingAsync(Event _event, User user, CancellationToken ct);
        Task DeleteAttendingAsync(AttendingInfo info, CancellationToken ct);
    }
}
