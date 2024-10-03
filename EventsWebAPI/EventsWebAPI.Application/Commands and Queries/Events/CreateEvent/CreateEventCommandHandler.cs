using AutoMapper;
using EventsWebAPI.Core.Exceptions;
using EventsWebAPI.Core.Models;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand>
    {
        private readonly IEventRepository _repo;
        private readonly IValidator<CreateEventCommand> _createValidator;
        private readonly IMapper _mapper;
        public CreateEventCommandHandler(IEventRepository repo, IValidator<CreateEventCommand> createValidator, IMapper mapper)
        {
            _repo = repo;
            _createValidator = createValidator;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(CreateEventCommand command, CancellationToken ct)
        {
            var modelState = _createValidator.Validate(command);
            if (!modelState.IsValid)
            {
                throw new BadRequestException("Check if you put valid data.");
            }
            var _event = _mapper.Map<Event>(command);
            if (await _repo.HasAnySameEventsAsync(_event, ct))
            {
                throw new NotAcceptableException("The event with the data you put already exists");
            }
            await _repo.CreateEventAsync(_event, ct);
            return Unit.Value;
        }
    }
}
