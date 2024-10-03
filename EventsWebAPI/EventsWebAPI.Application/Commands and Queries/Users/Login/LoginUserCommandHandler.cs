using EventsWebAPI.Application.Jwt.JwtTokenProviderService;
using EventsWebAPI.Core.Exceptions;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Application.Commands_and_Queries.Users.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _repo;
        private readonly JwtTokenProviderService _jwtTokenProvider;
        public LoginUserCommandHandler(IUserRepository repo, JwtTokenProviderService jwtTokenProvider)
        {
            _repo = repo;
            _jwtTokenProvider = jwtTokenProvider;
        }
        public async Task<string> Handle(LoginUserCommand request, CancellationToken ct)
        {
            var data = await _repo.GetUserByEmailAsync(request.Email, ct);
            if (data == null)
            {
                throw new BadRequestException("You seems have put the wrong email or the user with the email doesn't exist.");
            }
            string token = _jwtTokenProvider.GenerateToken(data);
            return token;
        }
    }
}
