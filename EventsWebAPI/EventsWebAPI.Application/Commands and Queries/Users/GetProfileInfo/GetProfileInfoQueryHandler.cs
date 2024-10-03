using AutoMapper;
using EventsWebAPI.Application.Jwt.JwtDataProviderService;
using EventsWebAPI.Core.Exceptions;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Application.Commands_and_Queries.Users.GetProfileInfo
{
    public class GetProfileInfoQueryHandler : IRequestHandler<GetProfileInfoQuery, GetProfileInfoResponse>
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly JwtDataProviderService _jwtDataProvider;
        public GetProfileInfoQueryHandler(IUserRepository repo, IMapper mapper, JwtDataProviderService jwtDataProvider)
        {
            _repo = repo;
            _mapper = mapper;
            _jwtDataProvider = jwtDataProvider;
        }
        public async Task<GetProfileInfoResponse> Handle(GetProfileInfoQuery query, CancellationToken ct)
        {
            if (query.HeaderData.Value.ToString().Trim() == "")
            {
                throw new BadRequestException();
            }
            Guid UserID = _jwtDataProvider.GetUserIDFromToken(query.HeaderData);
            var data = await _repo.GetUserByIdAsync(UserID, ct);
            var displayData = _mapper.Map<GetProfileInfoResponse>(data);
            return displayData;
        }
    }
}
