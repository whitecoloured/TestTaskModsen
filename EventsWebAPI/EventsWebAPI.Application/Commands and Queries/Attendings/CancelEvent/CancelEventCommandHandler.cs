using EventsWebAPI.Application.Jwt.JwtDataProviderService;
using EventsWebAPI.Core.Exceptions;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Application.Commands_and_Queries.Attendings.CancelEvent
{
    public class CancelEventCommandHandler : IRequestHandler<CancelEventCommand>
    {
        private readonly IAttendingRepository _repo;
        private readonly JwtDataProviderService _jwtDataProvider;
        public CancelEventCommandHandler(IAttendingRepository repo, JwtDataProviderService jwtDataProvider)
        {
            _repo = repo;
            _jwtDataProvider = jwtDataProvider;
        }
        public async Task<Unit> Handle(CancelEventCommand command, CancellationToken ct)
        {
            if (command.HeaderData.Value.ToString().Trim() == "")
            {
                throw new BadRequestException();
            }
            Guid UserID = _jwtDataProvider.GetUserIDFromToken(command.HeaderData);
            var data = await _repo.GetAttendingAsync(command.EventID, UserID, ct);
            if (data == null)
            {
                throw new NotFoundException();
            }
            await _repo.DeleteAttendingAsync(data, ct);
            return Unit.Value;
        }
    }
}
