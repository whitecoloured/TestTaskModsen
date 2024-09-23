using EventsWebAPI.Application.Jwt.JwtDataProviderService;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using EventsWebAPI.Application.Dto_s.Responses.Members;
using EventsWebAPI.Application.Dto_s.Responses.Events;
using EventsWebAPI.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using AutoMapper;

namespace EventsWebAPI.Application.Services
{
    public class AttendingService
    {
        private readonly IUserRepository _userRepo;
        private readonly IEventRepository _eventRepo;
        private readonly IAttendingRepository _attRepo;
        private readonly JwtDataProviderService _jwtDataProvider;
        private readonly IMapper _mapper;
        public AttendingService(IUserRepository userRepo, IEventRepository eventRepo, IAttendingRepository attRepo, JwtDataProviderService jwtDataProvider, IMapper mapper)
        {
            _userRepo = userRepo;
            _eventRepo = eventRepo;
            _attRepo = attRepo;
            _jwtDataProvider = jwtDataProvider;
            _mapper = mapper;
        }

        public async Task<List<MemberDto>> GetMembersByEvent(Guid EventID)
        {
            var data = await _attRepo.GetMembersByEventAsync(EventID);
            return data
                .Select(u=> _mapper.Map<MemberDto>(u.User))
                .ToList();
        }

        public async Task<List<MemberEventDTO>> GetMembersEvents(KeyValuePair<string, StringValues> headerData)
        {
            if (headerData.Value.ToString().Trim() == "")
            {
                throw new BadRequestException();
            }
            Guid UserID = _jwtDataProvider.GetUserIDFromToken(headerData);
            var data = await _attRepo.GetMembersEventsAsync(UserID);
            return data
                .Select(e => _mapper.Map<MemberEventDTO>(e))
                .ToList();
        }

        public async Task SubscribeToEvent(Guid EventID, KeyValuePair<string, StringValues> headerData, CancellationToken ct)
        {
            if (headerData.Value.ToString().Trim() == "")
            {
                throw new BadRequestException();
            }
            Guid UserID = _jwtDataProvider.GetUserIDFromToken(headerData);
            if (await HasUserAlreadySubscribed(EventID,UserID, ct))
            {
                throw new NotAcceptableException("You have already subscribed to the event!");
            }
            var _event = await _eventRepo.GetEventByIdAsync(EventID, ct);
            var user = await _userRepo.GetUserByIdAsync(UserID, ct);
            if (_event == null || user == null)
            {
                throw new NotFoundException();
            }
            await _attRepo.CreateAttendingAsync(_event, user, ct);
        }

        public async Task CancelEvent(Guid EventID, KeyValuePair<string, StringValues> headerData, CancellationToken ct)
        {
            if (headerData.Value.ToString().Trim() == "")
            {
                throw new BadRequestException();
            }
            Guid UserID = _jwtDataProvider.GetUserIDFromToken(headerData);
            var data = await _attRepo.GetAttendingAsync(EventID, UserID, ct);
            if (data==null)
            {
                throw new NotFoundException();
            }
            await _attRepo.DeleteAttendingAsync(data, ct);
        }

        private async Task<bool> HasUserAlreadySubscribed(Guid EventID, Guid UserID, CancellationToken ct)
        {
            var checkData = await _attRepo.GetAttendingAsync(EventID, UserID, ct);
            if (checkData != null)
            {
                return true;
            }
            else return false;
        }
    }
}
