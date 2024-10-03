using AutoMapper;
using EventsWebAPI.Application.Jwt.JwtDataProviderService;
using EventsWebAPI.Core.Exceptions;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Application.Commands_and_Queries.Attendings.GetMembersEvents
{
    public class GetMembersEventsQueryHandler : IRequestHandler<GetMembersEventsQuery, List<MemberEventResponse>>
    {
        private readonly IAttendingRepository _repo;
        private readonly JwtDataProviderService _jwtDataProvider;
        private readonly IMapper _mapper;
        public GetMembersEventsQueryHandler(IAttendingRepository repo, JwtDataProviderService jwtDataProvider, IMapper mapper)
        {
            _repo = repo;
            _jwtDataProvider = jwtDataProvider;
            _mapper = mapper;
        }
        public async Task<List<MemberEventResponse>> Handle(GetMembersEventsQuery query, CancellationToken cancellationToken)
        {
            if (query.HeaderData.Value.ToString().Trim() == "")
            {
                throw new BadRequestException();
            }
            Guid UserID = _jwtDataProvider.GetUserIDFromToken(query.HeaderData);
            var data = await _repo.GetMembersEventsAsync(UserID);
            return data
                .Select(e => _mapper.Map<MemberEventResponse>(e))
                .ToList();
        }
    }
}
