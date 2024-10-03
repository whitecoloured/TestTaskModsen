using AutoMapper;
using EventsWebAPI.Application.Jwt.JwtTokenProviderService;
using EventsWebAPI.Core.Exceptions;
using EventsWebAPI.Core.Models;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventsWebAPI.Application.Commands_and_Queries.Users.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUserRepository _repo;
        private readonly JwtTokenProviderService _jwtTokenProvider;
        private readonly IValidator<RegisterUserCommand> _validator;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IUserRepository repo, JwtTokenProviderService jwtTokenProvider, IValidator<RegisterUserCommand> validator, IMapper mapper)
        {
            _repo = repo;
            _jwtTokenProvider = jwtTokenProvider;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<string> Handle(RegisterUserCommand command, CancellationToken ct)
        {
            var data = await _repo.GetUserByEmailAsync(command.Email, ct);
            if (data != null)
            {
                throw new NotAcceptableException("You can't register with the email that already exists!");
            }
            var modelState = _validator.Validate(command);

            if (!modelState.IsValid)
            {
                throw new BadRequestException("Check if you put valid data.");
            }

            var user = _mapper.Map<User>(command);
            await _repo.CreateUserAsync(user, ct);

            var getUserData = await _repo.GetUserByEmailAsync(command.Email, ct);
            string token = _jwtTokenProvider.GenerateToken(getUserData);
            return token;
        }
    }
}
