using AutoMapper;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Application.Commands_and_Queries.Events.GetEventByIdWithMembers
{
    public class GetEventByIdWithMembersQueryHandler : IRequestHandler<GetEventByIdWithMembersQuery, EventInfoResponse>
    {
        private readonly IEventRepository _repo;
        private readonly IMapper _mapper;
        public GetEventByIdWithMembersQueryHandler( IEventRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<EventInfoResponse> Handle(GetEventByIdWithMembersQuery query, CancellationToken ct)
        {
            var data = await _repo.GetEventByIdIncludingMembersAsync(query.ID, ct);
            var displayData = _mapper.Map<EventInfoResponse>(data);
            return displayData;
        }
    }
}
