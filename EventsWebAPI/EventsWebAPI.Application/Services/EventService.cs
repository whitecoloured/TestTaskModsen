using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using EventsWebAPI.Application.Dto_s.Requests.Event;
using EventsWebAPI.Application.Dto_s.Responses.Events;
using EventsWebAPI.Core.Exceptions;
using FluentValidation;
using AutoMapper;
using EventsWebAPI.Core.Models;

namespace EventsWebAPI.Application.Services
{
    public class EventService
    {
        private readonly IEventRepository _repo;
        private readonly IValidator<CreateEventRequest> _createValidator;
        private readonly IValidator<UpdateEventRequest> _updateValidator;
        private readonly IMapper _mapper;

        public EventService(IEventRepository repo, IValidator<CreateEventRequest> createValidator, IValidator<UpdateEventRequest> updateValidator, IMapper mapper)
        {
            _repo = repo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public async Task<DisplayEventsDTO> GetAllEventsAsync(SearchEventsRequest searchRequest, FilterEventsRequest filterRequest, int page)
        {
            var data = await _repo.GetAllEventsAsQueryableAsync();

            if (!string.IsNullOrWhiteSpace(searchRequest.SearchValue))
            {
                data = _repo.GetEventsBySearch(data, searchRequest.SearchValue);
            }

            if (searchRequest.SearchCategory.HasValue)
            {
                data = _repo.GetEventsByCategory(data, searchRequest.SearchCategory);
            }

            if (searchRequest.SearchDate.HasValue)
            {
                data = _repo.GetEventsByDate(data, searchRequest.SearchDate);
            }

            if (!string.IsNullOrWhiteSpace(filterRequest.SortItem))
            {
                data = _repo.GetSortedEvents(data, filterRequest.SortItem, filterRequest.OrderByAsc);
            }

            int eventsAmount = data.Count();

            if (page!=-1)
            {
                data = _repo.GetEventsByPage(data, page);
            }

            var eventsList = data.Select(e => _mapper.Map<ReadEventDto>(e))
                            .ToList();

            return new DisplayEventsDTO() { EventsList = eventsList, EventsAmount = eventsAmount };
        }

        public async Task<GetEventDTO> GetEventById(Guid ID, CancellationToken ct)
        {
            var data = await _repo.GetEventByIdAsync(ID, ct);
            var displayData = _mapper.Map<GetEventDTO>(data);
            return displayData;
        }

        public async Task<EventInfoDto> GetEventWithMembersById(Guid ID, CancellationToken ct)
        {
            var data= await _repo.GetEventByIdIncludingMembersAsync(ID, ct);
            var displayData = _mapper.Map<EventInfoDto>(data);
            return displayData;
        }

        public async Task CreateEvent(CreateEventRequest request, CancellationToken ct)
        {
            var modelState = _createValidator.Validate(request);
            if (!modelState.IsValid)
            {
                throw new BadRequestException("Check if you put valid data.");
            }
            var _event = _mapper.Map<Event>(request);
            if (await _repo.HasAnySameEventsAsync(_event, ct))
            {
                throw new NotAcceptableException("The event with the data you put already exists");
            }
            await _repo.CreateEventAsync(_event, ct);
        }

        public async Task UpdateEvent(Guid ID,UpdateEventRequest request, CancellationToken ct)
        {
            var data = await _repo.GetEventByIdAsync(ID, ct);
            if (data==null)
            {
                throw new NotFoundException();
            }
            var modelState = _updateValidator.Validate(request);
            if (!modelState.IsValid)
            {
                throw new BadRequestException("Check if you put valid data.");
            }
            var _eventCheck = _mapper.Map<Event>(request);
            if (await _repo.HasAnySameEventsAsync(_eventCheck, ct)
                || await IsMaxAmountLessThanMembersAmount(ID, request.MaxAmountOfMembers, ct))
            {
                throw new NotAcceptableException("The event with the data you put already exists.\nAlso check if the number of members you are trying to put isn't lower than the actual amount of members");
            }

            data.Name = request.Name;
            data.Description = request.Description;
            data.EventDate = request.EventDate;
            data.EventPlace = request.EventPlace;
            data.Category = request.Category;
            data.MaxAmountOfMembers = request.MaxAmountOfMembers;
            data.ImageURL = request.ImageURL;
            await _repo.UpdateEventAsync(data, ct);
        }

        public async Task DeleteEvent(Guid ID, CancellationToken ct)
        {
            var data = await _repo.GetEventByIdAsync(ID, ct);
            if (data==null)
            {
                throw new NotFoundException();
            }
            await _repo.DeleteEventAsync(data, ct);
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
