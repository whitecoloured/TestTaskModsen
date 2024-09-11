using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsWebAPI.Dto_s.Events;
using EventsWebAPI.Requests.Event;
using EventsWebAPI.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using EventsWebAPI.Repositories.Interfaces;
using EventsWebAPI.Repositories.Implementations;

namespace EventsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _repository;
        private readonly IValidator<CreateEventRequest> _createValidator;
        private readonly IValidator<UpdateEventRequest> _updateValidator;
        public EventsController(IEventRepository repository,IValidator<CreateEventRequest> createValidator, IValidator<UpdateEventRequest> updateValidator)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }


        [HttpGet]
        [Route("GetAllEvents")]
        public async Task<IActionResult> GetAllEvents([FromQuery]SearchEventsRequest searchRequest,
            [FromQuery]FilterEventsRequest filterRequest,
            int page = -1)
        {

            var data = await _repository.GetAllEventsAsQueryableAsync(searchRequest, filterRequest);

            int eventsAmount = data.Count();

            if (page!=-1)
            {
                data = _repository.GetEventsByPage(data, page);
            }

            var displayData = await data
                                .Select(e => e.ToEventDto())
                                .ToListAsync();

            return Ok(new DisplayEventsDTO() { EventsList=displayData, EventsAmount=eventsAmount});
        }

        [HttpGet]
        [Route("GetEventById")]
        public async Task<IActionResult> GetEventById([Required] Guid id)
        {
            var data = await _repository.GetEventByIdAsync(id);
            return Ok(data.ToGetEventDto());
        }

        [HttpGet]
        [Route("GetEventInfoById")]
        public async Task<IActionResult> GetEventInfoById([Required]Guid id)
        {
            var data = await _repository.GetEventByIdIncludingMembersAsync(id);
            return Ok(data.ToEventInfoDto());
        }
        [HttpPost]
        [Authorize(policy:"AdminPolicy")]
        [Route("AddEvent")]
        public async Task<IActionResult> AddEvent(CreateEventRequest request)
        {
            var modelState = _createValidator.Validate(request);
            if (!modelState.IsValid)
            {
                return BadRequest();
            }

            if (await _repository.HasAnySameEventsAsync(request))
            {
                return StatusCode(406);
            }

            await _repository.CreateEventAsync(request);
            return Ok();
        }
        [HttpPut]
        [Authorize(policy: "AdminPolicy")]
        [Route("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent([Required]Guid EventID, UpdateEventRequest request)
        {

            var data = await _repository.GetEventByIdAsync(EventID);
            if (data==null)
            {
                return NotFound();
            }

            var modelState = _updateValidator.Validate(request);
            if (!modelState.IsValid)
            {
                return BadRequest();
            }


            if (await _repository.HasAnySameEventsAsync(request)
                || await IsMaxAmountLessThanMembersAmount(EventID, request.MaxAmountOfMembers))
            {
                return StatusCode(406);
            }

            await _repository.UpdateEventAsync(data, request);
            return Ok();
        }
        [HttpDelete]
        [Authorize(policy: "AdminPolicy")]
        [Route("DeleteEvent")]
        public async Task<IActionResult> DeleteEvent([Required]Guid EventID)
        {
            var data = await _repository.GetEventByIdAsync(EventID);
            if (data == null)
            {
                return NotFound();
            }
            await _repository.DeleteEventAsync(data);
            return Ok();
        }

        private async Task<bool> IsMaxAmountLessThanMembersAmount(Guid ID,int MaxAmountOfMembers)
        {
            var dataWithMembers = await _repository.GetEventByIdIncludingMembersAsync(ID);
            int actualMembersAmount = dataWithMembers.Members.Count;
            if (actualMembersAmount > MaxAmountOfMembers)
            {
                return true;
            }
            else return false;
        }
    }
}
