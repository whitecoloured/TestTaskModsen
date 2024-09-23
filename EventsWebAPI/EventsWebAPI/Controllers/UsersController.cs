using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventsWebAPI.Application.Dto_s.Requests.User;
using EventsWebAPI.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EventsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;
        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(policy: "UserPolicy")]
        [Route("GetProfileInfo")]
        public async Task<IActionResult> GetProfileInfo(CancellationToken ct)
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            var userData = await _service.GetProfileInfo(headerData, ct);
            return Ok(userData);
        }

        [HttpGet]
        [Authorize(policy: "UserPolicy")]
        [Route("GetUserRole")]
        public IActionResult GetUserRole()
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            string role = _service.GetUserRole(headerData);
            return Ok(role);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken ct)
        {
            string token = await _service.Register(request, ct);
            return Ok(token);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginUserRequest request, CancellationToken ct)
        {
            string token = await _service.Login(request, ct);
            return Ok(token);
        }

        [HttpDelete]
        [Authorize(policy: "UserPolicy")]
        [Route("DeleteUserProfile")]
        public async Task<IActionResult> DeleteUserProfile(CancellationToken ct)
        {
            var headerData=Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            await _service.DeleteUserProfile(headerData, ct);
            return Ok();
        }

    }
}
