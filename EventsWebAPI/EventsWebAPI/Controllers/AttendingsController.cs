using EventsWebAPI.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace EventsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendingsController : ControllerBase
    {
        private readonly AttendingService _service;
        public AttendingsController(AttendingService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetMembersByEvent")]
        public async Task<IActionResult> GetMembersByEvent([Required]Guid EventID)
        {
            var data = await _service.GetMembersByEvent(EventID);
            
            return Ok(data);
        }
        [HttpGet]
        [Authorize(policy: "UserPolicy")]
        [Route("GetMembersEvents")]
        public async Task<IActionResult> GetMembersEvents()
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            var data = await _service.GetMembersEvents(headerData);

            return Ok(data);
        }
        [HttpPost]
        [Authorize(policy: "UserPolicy")]
        [Route("SubscribeToEvent")]
        public async Task<IActionResult> SubcribeToEvent([Required]Guid EventID, CancellationToken ct)
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            await _service.SubscribeToEvent(EventID, headerData, ct);
            return Ok();
        }

        [HttpDelete]
        [Authorize(policy: "UserPolicy")]
        [Route("CancelEvent")]
        public async Task<IActionResult> CancelEvent([Required]Guid EventID, CancellationToken ct)
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            await _service.CancelEvent(EventID, headerData, ct);
            return Ok();
        }
    }
}
