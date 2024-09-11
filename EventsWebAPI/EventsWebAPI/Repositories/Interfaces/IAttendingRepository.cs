using EventsWebAPI.Models;
using EventsWebAPI.Dto_s.Events;
using EventsWebAPI.Dto_s.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsWebAPI.Repositories.Interfaces
{
    public interface IAttendingRepository
    {
        Task<List<MemberEventDTO>> GetMembersEventsAsync(Guid UserID);
        Task<List<MemberDto>> GetMembersByEventAsync(Guid EventID);
        Task<AttendingInfo> GetAttendingAsync(Guid EventID, Guid UserID);
        Task CreateAttendingAsync(Event _event, User user);
        Task DeleteAttendingAsync(AttendingInfo info);
    }
}
