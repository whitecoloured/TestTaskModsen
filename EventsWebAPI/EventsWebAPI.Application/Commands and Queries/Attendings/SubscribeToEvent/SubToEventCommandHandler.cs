using EventsWebAPI.Application.Jwt.JwtDataProviderService;
using EventsWebAPI.Core.Exceptions;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Application.Commands_and_Queries.Attendings.SubscribeToEvent
{
    public class SubToEventCommandHandler : IRequestHandler<SubToEventCommand>
    {
        private readonly IAttendingRepository _attRepo;
        private readonly IEventRepository _eventRepo;
        private readonly IUserRepository _userRepo;
        private readonly JwtDataProviderService _jwtDataProvider;

        public SubToEventCommandHandler(IAttendingRepository attRepo, IEventRepository eventRepo, IUserRepository userRepo, JwtDataProviderService jwtDataProvider)
        {
            _attRepo = attRepo;
            _eventRepo = eventRepo;
            _userRepo = userRepo;
            _jwtDataProvider = jwtDataProvider;
        }
        public async Task<Unit> Handle(SubToEventCommand command, CancellationToken ct)
        {
            if (command.HeaderData.Value.ToString().Trim() == "")
            {
                throw new BadRequestException();
            }
            Guid UserID = _jwtDataProvider.GetUserIDFromToken(command.HeaderData);
            if (await HasUserAlreadySubscribed(command.EventID, UserID, ct))
            {
                throw new NotAcceptableException("You have already subscribed to the event!");
            }
            var _event = await _eventRepo.GetEventByIdAsync(command.EventID, ct);
            var user = await _userRepo.GetUserByIdAsync(UserID, ct);
            if (_event == null || user == null)
            {
                throw new NotFoundException();
            }
            await _attRepo.CreateAttendingAsync(_event, user, ct);
            return Unit.Value;
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
