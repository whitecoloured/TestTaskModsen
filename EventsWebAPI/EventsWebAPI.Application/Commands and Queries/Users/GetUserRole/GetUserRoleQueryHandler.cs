using EventsWebAPI.Application.Jwt.JwtDataProviderService;
using EventsWebAPI.Core.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Application.Commands_and_Queries.Users.GetUserRole
{
    public class GetUserRoleQueryHandler : IRequestHandler<GetUserRoleQuery, string>
    {
        private readonly JwtDataProviderService _jwtDataProvider;
        public GetUserRoleQueryHandler(JwtDataProviderService jwtDataProvider)
        {
            _jwtDataProvider = jwtDataProvider;
        }
        public Task<string> Handle(GetUserRoleQuery query, CancellationToken cancellationToken)
        {
            if (query.HeaderData.Value.ToString().Trim() == "")
            {
                throw new BadRequestException();
            }
            string role = _jwtDataProvider.GetUserRoleFromToken(query.HeaderData);
            return Task.FromResult(role);
        }
    }
}
