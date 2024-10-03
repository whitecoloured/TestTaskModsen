using MediatR;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using EventsWebAPI.Core.Exceptions;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IEventRepository _repo;
        public DeleteEventCommandHandler(IEventRepository repo)
        {
            _repo = repo;
        }
        public async Task<Unit> Handle(DeleteEventCommand command, CancellationToken ct)
        {
            var data = await _repo.GetEventByIdAsync(command.ID, ct);
            if (data == null)
            {
                throw new NotFoundException();
            }
            await _repo.DeleteEventAsync(data, ct);
            return Unit.Value;
        }
    }
}
