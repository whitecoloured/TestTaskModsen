using AutoMapper;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.GetEventById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, GetEventByIdResponse>
    {
        private readonly IEventRepository _repo;
        private readonly IMapper _mapper;
        public GetEventByIdQueryHandler(IEventRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<GetEventByIdResponse> Handle(GetEventByIdQuery query, CancellationToken ct)
        {
            var data = await _repo.GetEventByIdAsync(query.ID, ct);
            var displayData = _mapper.Map<GetEventByIdResponse>(data);
            return displayData;
        }
    }
}
