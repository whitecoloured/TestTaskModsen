using EventsWebAPI.Application.Dto_s.Requests.User;
using EventsWebAPI.Application.Dto_s.Responses.Users;
using EventsWebAPI.Core.Exceptions;
using EventsWebAPI.Infrastructure.Repositories.Interfaces;
using EventsWebAPI.Application.Jwt.JwtTokenProviderService;
using EventsWebAPI.Application.Jwt.JwtDataProviderService;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using System;
using AutoMapper;
using EventsWebAPI.Core.Models;

namespace EventsWebAPI.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        private readonly JwtTokenProviderService _jwtTokenProvider;
        private readonly JwtDataProviderService _jwtDataProvider;
        private readonly IValidator<RegisterUserRequest> _validator;
        private readonly IMapper _mapper;
        public UserService(IUserRepository repo, IValidator<RegisterUserRequest> validator, JwtTokenProviderService jwtTokenProvider, JwtDataProviderService jwtDataProvider, IMapper mapper)
        {
            _repo = repo;
            _validator = validator;
            _jwtTokenProvider = jwtTokenProvider;
            _jwtDataProvider = jwtDataProvider;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetProfileInfo(KeyValuePair<string, StringValues> headerData, CancellationToken ct)
        {
            if (headerData.Value.ToString().Trim() == "")
            {
                throw new BadRequestException();
            }
            Guid UserID = _jwtDataProvider.GetUserIDFromToken(headerData);
            var data = await _repo.GetUserByIdAsync(UserID,ct);
            var displayData = _mapper.Map<UserDTO>(data);
            return displayData;
        }

        public string GetUserRole(KeyValuePair<string, StringValues> headerData)
        {
            if (headerData.Value.ToString().Trim() == "")
            {
                throw new BadRequestException();
            }
            string role = _jwtDataProvider.GetUserRoleFromToken(headerData);
            return role;
        }
        public async Task<string> Register(RegisterUserRequest request, CancellationToken ct)
        {
            var data = await _repo.GetUserByEmailAsync(request.Email, ct);
            if (data != null)
            {
                throw new NotAcceptableException("You can't register with the email that already exists!");
            }
            var modelState = _validator.Validate(request);
            if (!modelState.IsValid)
            {
                throw new BadRequestException("Check if you put valid data.");
            }
            var user = _mapper.Map<User>(request);
            await _repo.CreateUserAsync(user, ct);

            var getUserData = await _repo.GetUserByEmailAsync(request.Email, ct);
            string token = _jwtTokenProvider.GenerateToken(getUserData);
            return token;
        }
        
        public async Task<string> Login(LoginUserRequest request, CancellationToken ct)
        {
            var data = await _repo.GetUserByEmailAsync(request.Email, ct);
            if (data==null)
            {
                throw new BadRequestException("You seems have put the wrong email or the user with the email doesn't exist.");
            }
            string token = _jwtTokenProvider.GenerateToken(data);
            return token;
        }

        public async Task DeleteUserProfile(KeyValuePair<string, StringValues> headerData, CancellationToken ct)
        {
            if (headerData.Value.ToString().Trim() == "")
            {
                throw new BadRequestException();
            }
            Guid UserID = _jwtDataProvider.GetUserIDFromToken(headerData);
            var data = await _repo.GetUserByIdAsync(UserID, ct);
            if (data==null)
            {
                throw new NotFoundException();
            }
            await _repo.DeleteUserAsync(data, ct);
        }
    }
}
