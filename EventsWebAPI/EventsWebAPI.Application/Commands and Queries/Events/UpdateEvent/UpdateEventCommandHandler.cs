using System;
using MediatR;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using FluentValidation;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using EventsWebAPI.Core.Exceptions;
using EventsWebAPI.Core.Models;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IEventRepository _repo;
        private readonly IValidator<UpdateEventRequest> _updateValidator;
        private readonly IMapper _mapper;
        public UpdateEventCommandHandler(IEventRepository repo, IValidator<UpdateEventRequest> updateValidator, IMapper mapper)
        {
            _repo = repo;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateEventCommand command, CancellationToken ct)
        {
            var data = await _repo.GetEventByIdAsync(command.ID, ct);
            if (data == null)
            {
                throw new NotFoundException();
            }
            var modelState = _updateValidator.Validate(command.Request);
            if (!modelState.IsValid)
            {
                throw new BadRequestException("Check if you put valid data.");
            }
            var _eventCheck = _mapper.Map<Event>(command.Request);
            if (await _repo.HasAnySameEventsAsync(_eventCheck, ct)
                || await IsMaxAmountLessThanMembersAmount(command.ID, command.Request.MaxAmountOfMembers, ct))
            {
                throw new NotAcceptableException("The event with the data you put already exists.\nAlso check if the number of members you are trying to put isn't lower than the actual amount of members");
            }

            data.Name = command.Request.Name;
            data.Description = command.Request.Description;
            data.EventDate = command.Request.EventDate;
            data.EventPlace = command.Request.EventPlace;
            data.Category = command.Request.Category;
            data.MaxAmountOfMembers = command.Request.MaxAmountOfMembers;
            data.ImageURL = command.Request.ImageURL;
            await _repo.UpdateEventAsync(data, ct);
            return Unit.Value;
        }

        private async Task<bool> IsMaxAmountLessThanMembersAmount(Guid ID, int MaxAmountOfMembers, CancellationToken ct)
        {
            var dataWithMembers = await _repo.GetEventByIdIncludingMembersAsync(ID, ct);
            int actualMembersAmount = dataWithMembers.Members.Count;
            if (actualMembersAmount > MaxAmountOfMembers)
            {
                return true;
            }
            else return false;
        }
    }
}
