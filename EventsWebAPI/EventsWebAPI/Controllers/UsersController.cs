using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using EventsWebAPI.Application.Commands_and_Queries.Users.GetProfileInfo;
using EventsWebAPI.Application.Commands_and_Queries.Users.GetUserRole;
using EventsWebAPI.Application.Commands_and_Queries.Users.Register;
using EventsWebAPI.Application.Commands_and_Queries.Users.Login;
using EventsWebAPI.Application.Commands_and_Queries.Users.DeleteUserProfile;

namespace EventsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;
        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Authorize(policy: "UserPolicy")]
        [Route("GetProfileInfo")]
        public async Task<IActionResult> GetProfileInfo(CancellationToken ct)
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            GetProfileInfoQuery query = new(headerData);
            var userData = await _sender.Send(query, ct);
            return Ok(userData);
        }

        [HttpGet]
        [Authorize(policy: "UserPolicy")]
        [Route("GetUserRole")]
        public IActionResult GetUserRole()
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            GetUserRoleQuery query = new(headerData);
            string role = _sender.Send(query).Result;
            return Ok(role);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken ct)
        {
            string token = await _sender.Send(command, ct);
            return Ok(token);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken ct)
        {
            string token = await _sender.Send(command, ct);
            return Ok(token);
        }

        [HttpDelete]
        [Authorize(policy: "UserPolicy")]
        [Route("DeleteUserProfile")]
        public async Task<IActionResult> DeleteUserProfile(CancellationToken ct)
        {
            var headerData=Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            DeleteUserCommand command = new(headerData);
            var result = await _sender.Send(command, ct);
            return Ok(result);
        }

    }
}
