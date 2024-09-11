using EventsWebAPI.Models;
using EventsWebAPI.Dto_s.Members;
using EventsWebAPI.Dto_s.Events;
using EventsWebAPI.Jwt.JwtDataProviderService;
using EventsWebAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace EventsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendingsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IAttendingRepository _attendingRepository;
        private readonly JwtDataProviderService _jwtDataProvider;
        public AttendingsController(JwtDataProviderService jwtDataProvider, IUserRepository userRepo, IEventRepository eventRepo, IAttendingRepository attendingRepo)
        {
            _userRepository = userRepo;
            _eventRepository = eventRepo;
            _attendingRepository = attendingRepo;
            _jwtDataProvider = jwtDataProvider;
        }

        [HttpGet]
        [Route("GetMembersByEvent")]
        public async Task<IActionResult> GetMembersByEvent([Required]Guid EventID)
        {
            var data = await _attendingRepository.GetMembersByEventAsync(EventID);
            
            return Ok(data);
        }
        [HttpGet]
        [Authorize(policy: "UserPolicy")]
        [Route("GetMembersEvents")]
        public async Task<IActionResult> GetMembersEvents()
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            if (headerData.Value.ToString().Trim() == "")
            {
                return BadRequest();
            }
            var UserID = _jwtDataProvider.GetUserIDFromToken(headerData);
            var data = await _attendingRepository.GetMembersEventsAsync(UserID);

            return Ok(data);
        }
        [HttpPost]
        [Authorize(policy: "UserPolicy")]
        [Route("SubscribeToEvent")]
        public async Task<IActionResult> SubcribeToEvent([Required]Guid EventID)
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            if (headerData.Value.ToString().Trim() == "")
            {
                return BadRequest();
            }
            var UserID = _jwtDataProvider.GetUserIDFromToken(headerData);
            if (await HasUserAlreadySubscribed(EventID, UserID))
            {
                return StatusCode(406, "You already have subscribed to the event!");
            }
            var _event = await _eventRepository.GetEventByIdAsync(EventID);
            var user = await _userRepository.GetUserByIdAsync(UserID);
            if (_event==null || user==null)
            {
                return NotFound();
            }
            await _attendingRepository.CreateAttendingAsync(_event, user);
            return Ok();
        }

        [HttpDelete]
        [Authorize(policy: "UserPolicy")]
        [Route("CancelEvent")]
        public async Task<IActionResult> CancelEvent([Required]Guid EventID)
        {
            var headerData = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            if (headerData.Value.ToString().Trim() == "")
            {
                return BadRequest();
            }
            var UserID = _jwtDataProvider.GetUserIDFromToken(headerData);
            var data = await _attendingRepository.GetAttendingAsync(EventID, UserID);
            if (data==null)
            {
                return NotFound();
            }
            await _attendingRepository.DeleteAttendingAsync(data);
            return Ok();
        }

        private async Task<bool> HasUserAlreadySubscribed(Guid EventID, Guid UserID)
        {
            var checkData = await _attendingRepository.GetAttendingAsync(EventID, UserID);
            if (checkData != null)
            {
                return true;
            }
            else return false;
        }
    }
}
