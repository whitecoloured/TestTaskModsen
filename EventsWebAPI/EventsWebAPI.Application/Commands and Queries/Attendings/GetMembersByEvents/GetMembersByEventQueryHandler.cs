using AutoMapper;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Application.Commands_and_Queries.Attendings.GetMembersByEvents
{
    public class GetMembersByEventQueryHandler : IRequestHandler<GetMembersByEventQuery, List<EventMembersResponse>>
    {
        private readonly IAttendingRepository _repo;
        private readonly IMapper _mapper;
        public GetMembersByEventQueryHandler(IAttendingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<List<EventMembersResponse>> Handle(GetMembersByEventQuery query, CancellationToken cancellationToken)
        {
            var data = await _repo.GetMembersByEventAsync(query.ID);
            return data
                .Select(u => _mapper.Map<EventMembersResponse>(u.User))
                .ToList();
        }
    }
}
