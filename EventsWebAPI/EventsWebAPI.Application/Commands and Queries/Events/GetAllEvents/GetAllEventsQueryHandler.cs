using MediatR;
using EventsWebAPI.Application.Commands_and_Queries.Events.GetAllEvents.Responses;
using System.Threading.Tasks;
using System.Threading;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using AutoMapper;
using System.Linq;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.GetAllEvents
{
    public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, DisplayEventsDTO>
    {
        private readonly IEventRepository _repo;
        private readonly IMapper _mapper;
        public GetAllEventsQueryHandler(IEventRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<DisplayEventsDTO> Handle(GetAllEventsQuery query, CancellationToken cancellationToken)
        {
            var data = await _repo.GetAllEventsAsQueryableAsync();

            if (!string.IsNullOrWhiteSpace(query.SearchRequest?.SearchValue))
            {
                data = _repo.GetEventsBySearch(data, query.SearchRequest?.SearchValue);
            }
            
            if (query.SearchRequest?.SearchCategory is not null)
            {
                data = _repo.GetEventsByCategory(data, query.SearchRequest.SearchCategory);
            }

            if (query.SearchRequest?.SearchDate is not null)
            {
                data = _repo.GetEventsByDate(data, query.SearchRequest.SearchDate);
            }

            if (!string.IsNullOrWhiteSpace(query.FilterRequest?.SortItem))
            {
                data = _repo.GetSortedEvents(data, query.FilterRequest?.SortItem, query.FilterRequest.OrderByAsc);
            }

            int eventsAmount = data.Count();

            if (query.Page != -1)
            {
                data = _repo.GetEventsByPage(data, query.Page);
            }

            var eventsList = data.Select(e => _mapper.Map<ReadEventDto>(e))
                            .ToList();

            return new DisplayEventsDTO() { EventsList = eventsList, EventsAmount = eventsAmount };
        }
    }
}
