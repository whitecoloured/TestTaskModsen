using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using EventsWebAPI.Application.Commands_and_Queries.Events.GetAllEvents;
using EventsWebAPI.Application.Commands_and_Queries.Events.GetEventById;
using EventsWebAPI.Application.Commands_and_Queries.Events.GetEventByIdWithMembers;
using EventsWebAPI.Application.Commands_and_Queries.Events.CreateEvent;
using EventsWebAPI.Application.Commands_and_Queries.Events.UpdateEvent;
using EventsWebAPI.Application.Commands_and_Queries.Events.DeleteEvent;

namespace EventsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ISender _sender;
        public EventsController(ISender sender)
        {
            _sender = sender;
        }


        [HttpGet]
        [Route("GetAllEvents")]
        public async Task<IActionResult> GetAllEvents([FromQuery]GetAllEventsQuery query)
        {
            var data = await _sender.Send(query);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetEventById")]
        public async Task<IActionResult> GetEventById([FromQuery]GetEventByIdQuery query, CancellationToken ct)
        {
            var data = await _sender.Send(query, ct);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetEventInfoById")]
        public async Task<IActionResult> GetEventInfoById([FromQuery]GetEventByIdWithMembersQuery query, CancellationToken ct)
        {
            var data = await _sender.Send(query, ct);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(policy:"AdminPolicy")]
        [Route("AddEvent")]
        public async Task<IActionResult> AddEvent(CreateEventCommand command, CancellationToken ct)
        {
            var result=await _sender.Send(command, ct);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(policy: "AdminPolicy")]
        [Route("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent(UpdateEventCommand command, CancellationToken ct)
        {

            var result = await _sender.Send(command, ct);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(policy: "AdminPolicy")]
        [Route("DeleteEvent")]
        public async Task<IActionResult> DeleteEvent([FromQuery]DeleteEventCommand command, CancellationToken ct)
        {
            var result=await _sender.Send(command, ct);
            return Ok(result);
        }
    }
}
