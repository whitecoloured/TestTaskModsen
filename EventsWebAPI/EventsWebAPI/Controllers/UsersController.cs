using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EventsWebAPI.Dto_s.Users;
using EventsWebAPI.Requests.User;
using EventsWebAPI.Jwt.JwtTokenProviderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Primitives;
using EventsWebAPI.Jwt.JwtDataProviderService;
using FluentValidation;
using EventsWebAPI.Repositories.Interfaces;

namespace EventsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly JwtTokenProviderService _jwtProvider;
        private readonly JwtDataProviderService _jwtDataProvider;
        private readonly IValidator<RegisterUserRequest> _createValidator;

        public UsersController(IUserRepository repository, JwtTokenProviderService jwtProvider, JwtDataProviderService jwtDataProvider, IValidator<RegisterUserRequest> createValidator)
        {
            _repository = repository;
            _jwtProvider = jwtProvider;
            _jwtDataProvider = jwtDataProvider;
            _createValidator = createValidator;
        }

        [HttpGet]
        [Authorize(policy: "UserPolicy")]
        [Route("GetProfileInfo")]
        public async Task<IActionResult> GetProfileInfo()
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            if (headerData.Value.ToString().Trim()=="")
            {
                return BadRequest();
            }
            var UserID = _jwtDataProvider.GetUserIDFromToken(headerData);
            var data = await _repository.GetUserByIdAsync(UserID);
            return Ok(data.ToUserDto());
        }

        [HttpGet]
        [Authorize(policy: "UserPolicy")]
        [Route("GetUserRole")]
        public IActionResult GetUserRole()
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            if (headerData.Value.ToString().Trim() == "")
            {
                return BadRequest();
            }
            var Role = _jwtDataProvider.GetUserRoleFromToken(headerData);
            return Ok(Role);
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var data = await _repository.GetUserByEmailAsync(request.Email);
            if (data != null)
            {
                return StatusCode(406);
            }
            var modelState = _createValidator.Validate(request);
            if (!modelState.IsValid)
            {
                return BadRequest();
            }
            await _repository.CreateUserAsync(request);

            var user = await _repository.GetUserByEmailAsync(request.Email);
            var token = _jwtProvider.GenerateToken(user);

            return Ok(token);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var data = await _repository.GetUserByEmailAsync(request.Email);
            if (data==null)
            {
                return NotFound();
            }
            string token = _jwtProvider.GenerateToken(data);
            return Ok(token);
        }

        [HttpDelete]
        [Authorize(policy: "UserPolicy")]
        [Route("DeleteUserProfile")]
        public async Task<IActionResult> DeleteUserProfile()
        {
            var headerData=Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            if (headerData.Value.ToString().Trim() == "")
            {
                return BadRequest();
            }
            var UserID = _jwtDataProvider.GetUserIDFromToken(headerData);
            var data = await _repository.GetUserByIdAsync(UserID);
            if (data==null)
            {
                return NotFound();
            }
            await _repository.DeleteUserAsync(data);
            return Ok();
        }

    }
}
