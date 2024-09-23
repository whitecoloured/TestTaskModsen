using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using EventsWebAPI.Application.Dto_s.Requests.Event;
using EventsWebAPI.Application.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace EventsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventService _service;
        public EventsController(EventService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("GetAllEvents")]
        public async Task<IActionResult> GetAllEvents([FromQuery]SearchEventsRequest searchRequest,
            [FromQuery]FilterEventsRequest filterRequest,
            int page = -1)
        {

            var data = await _service.GetAllEventsAsync(searchRequest, filterRequest, page);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetEventById")]
        public async Task<IActionResult> GetEventById([Required] Guid id, CancellationToken ct)
        {
            var data = await _service.GetEventById(id, ct);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetEventInfoById")]
        public async Task<IActionResult> GetEventInfoById([Required]Guid id, CancellationToken ct)
        {
            var data = await _service.GetEventWithMembersById(id, ct);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(policy:"AdminPolicy")]
        [Route("AddEvent")]
        public async Task<IActionResult> AddEvent(CreateEventRequest request, CancellationToken ct)
        {
            await _service.CreateEvent(request, ct);
            return Ok();
        }

        [HttpPut]
        [Authorize(policy: "AdminPolicy")]
        [Route("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent([Required]Guid EventID, UpdateEventRequest request, CancellationToken ct)
        {

            await _service.UpdateEvent(EventID, request, ct);
            return Ok();
        }

        [HttpDelete]
        [Authorize(policy: "AdminPolicy")]
        [Route("DeleteEvent")]
        public async Task<IActionResult> DeleteEvent([Required]Guid EventID, CancellationToken ct)
        {
            await _service.DeleteEvent(EventID, ct);
            return Ok();
        }
    }
}
