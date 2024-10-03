using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using EventsWebAPI.Application.Commands_and_Queries.Attendings.GetMembersByEvents;
using EventsWebAPI.Application.Commands_and_Queries.Attendings.GetMembersEvents;
using EventsWebAPI.Application.Commands_and_Queries.Attendings.SubscribeToEvent;
using EventsWebAPI.Application.Commands_and_Queries.Attendings.CancelEvent;

namespace EventsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendingsController : ControllerBase
    {
        private readonly ISender _sender;
        public AttendingsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Route("GetMembersByEvent")]
        public async Task<IActionResult> GetMembersByEvent([FromQuery]GetMembersByEventQuery query)
        {
            var data = await _sender.Send(query);
            
            return Ok(data);
        }
        [HttpGet]
        [Authorize(policy: "UserPolicy")]
        [Route("GetMembersEvents")]
        public async Task<IActionResult> GetMembersEvents()
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            GetMembersEventsQuery query = new(headerData);
            var data = await _sender.Send(query);

            return Ok(data);
        }
        [HttpPost]
        [Authorize(policy: "UserPolicy")]
        [Route("SubscribeToEvent")]
        public async Task<IActionResult> SubcribeToEvent([Required]Guid EventID, CancellationToken ct)
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            SubToEventCommand command = new(EventID, headerData);
            var result = await _sender.Send(command, ct);
            return Ok();
        }

        [HttpDelete]
        [Authorize(policy: "UserPolicy")]
        [Route("CancelEvent")]
        public async Task<IActionResult> CancelEvent([Required]Guid EventID, CancellationToken ct)
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            CancelEventCommand command = new(EventID, headerData);
            var result = await _sender.Send(command,ct);
            return Ok(result);
        }
    }
}
