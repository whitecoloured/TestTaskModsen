using EventsWebAPI.Application.Jwt.JwtDataProviderService;
using EventsWebAPI.Core.Exceptions;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Application.Commands_and_Queries.Users.DeleteUserProfile
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _repo;
        private readonly JwtDataProviderService _jwtDataProvider;

        public DeleteUserCommandHandler(IUserRepository repo, JwtDataProviderService jwtDataProvider)
        {
            _repo = repo;
            _jwtDataProvider = jwtDataProvider;
        }
        
        public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken ct)
        {
            if (command.HeaderData.Value.ToString().Trim() == "")
            {
                throw new BadRequestException();
            }
            Guid UserID = _jwtDataProvider.GetUserIDFromToken(command.HeaderData);
            var data = await _repo.GetUserByIdAsync(UserID, ct);
            if (data == null)
            {
                throw new NotFoundException();
            }
            await _repo.DeleteUserAsync(data, ct);
            return Unit.Value;
        }
    }
}
